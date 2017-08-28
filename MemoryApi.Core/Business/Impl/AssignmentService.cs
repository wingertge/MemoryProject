using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MemoryApi.Core.Database.Repositories;
using MemoryApi.Core.DbModels;
using MemoryCore.Models;
using MemoryServer.Core.Business.Util;

namespace MemoryApi.Core.Business.Impl
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IScheduler _scheduler;
        private readonly IAuthenticationService _authService;
        private readonly ILessonReviewStore _reviewStore;
        private readonly IReviewRepository _reviewRepository;

        private readonly Random _random = new Random();

        public AssignmentService(IAssignmentRepository assignmentRepository, ILessonRepository lessonRepository, 
            IScheduler scheduler, IAuthenticationService authService, ILessonReviewStore reviewStore,
            IReviewRepository reviewRepository)
        {
            _assignmentRepository = assignmentRepository;
            _lessonRepository = lessonRepository;
            _scheduler = scheduler;
            _authService = authService;
            _reviewStore = reviewStore;
            _reviewRepository = reviewRepository;
        }

        public async Task<List<Assignment>> GetUserAssignments(string userId) => await _assignmentRepository.GetAssignmentsByUserAsync(userId);

        public async Task<Assignment> CreateOrEditAssignment(int langFromId, int langToId, string reading, string pronunciation, string meaning, string creatorId, Guid assignmentId)
        {
            var existingLesson = await _lessonRepository.FindLessonByContentAsync(langFromId, langToId, reading, pronunciation, meaning);
            if (existingLesson == null)
            {
                var newLesson = new Lesson
                {
                    Languages = new DbLanguagePair(langFromId, langToId),
                    Reading = reading,
                    Pronunciation = pronunciation,
                    Meaning = meaning
                };
                existingLesson = await _lessonRepository.CreateOrUpdateLessonAsync(newLesson);
            }

            var assignment = await _assignmentRepository.FindAssignmentByLessonIdAsync(creatorId, existingLesson.Id);
            if (assignment != null)
            {
                if (assignmentId != Guid.Empty) await _assignmentRepository.DeleteAssignmentByIdAsync(assignmentId);
                return null;
            }

            if (assignmentId != Guid.Empty) assignment = await _assignmentRepository.FindAssignmentByIdAsync(assignmentId);
            else
            {
                assignment = new Assignment
                {
                    UserId = creatorId,
                    Stage = 0,
                    Active = false
                };
            }

            assignment.Lesson = existingLesson;
            assignment.NextReview = _scheduler.GetNextReview(assignment, assignment.Stage);
            assignment = await _assignmentRepository.CreateOrUpdateAssignmentAsync(assignment);
            await _authService.SetLanguages(langFromId, langToId);
            
            return assignment;
        }

        public Task<int> GetPendingLessonReviewCount(string userId) => _assignmentRepository.GetPendingLessonCountAsync(userId);

        private async Task InitialiseHand(string userId)
        {
            var assignments = await _assignmentRepository.GetPendingLessonsAsync(userId);
            var context = new ReviewCardContext();
            assignments.ForEach(a =>
            {
                context.Add(new CardEntry { Assignment = a, FromField = ReviewField.Reading, ToField = ReviewField.Meaning });
                if (a.Lesson.Pronunciation == string.Empty)
                    context.Add(new CardEntry
                    {
                        Assignment = a,
                        FromField = ReviewField.Meaning,
                        ToField = ReviewField.Reading
                    });
                else
                {
                    context.Add(new CardEntry { Assignment = a, FromField = ReviewField.Reading, ToField = ReviewField.Pronunciation });
                    context.Add(new CardEntry { Assignment = a, FromField = ReviewField.Meaning, ToField = ReviewField.Pronunciation });
                }
            });

            context.FillHand(10);
            await _reviewStore.InitaliseUserHand(userId, context);
        }

        [ItemCanBeNull]
        public async Task<ReviewModel> GetNextReviewAsync(string userId)
        {
            var pendingReviews = await GetPendingLessonReviewCount(userId);
            if (pendingReviews == 0) return null;
            if (!await _reviewStore.DoesUserExist(userId))
                await InitialiseHand(userId);

            var currentHand = await _reviewStore.GetUserHand(userId);
            if (currentHand.Hand.Count == 0)
            {
                await InitialiseHand(userId);
                currentHand = await _reviewStore.GetUserHand(userId);
            }

            var random = _random.Next(0, currentHand.Hand.Count);
            var selectedCard = currentHand.Hand.ToList()[random];
            return new ReviewModel
            {
                AssignmentId = selectedCard.Assignment.Id,
                FieldFrom = selectedCard.FromField,
                FieldTo = selectedCard.ToField,
                From = selectedCard.Assignment.ValueFromFieldType(selectedCard.FromField),
                To = "",
                Solution = selectedCard.Assignment.ValueFromFieldType(selectedCard.ToField)
            };
        }

        public async Task<bool> SubmitReviewAsync(string userId, Guid id, ReviewField fieldFrom, ReviewField fieldTo, string input)
        {
            if (!await _reviewStore.DoesUserExist(userId)) return false;
            var assignment = await _assignmentRepository.GetAssignmentByIdAsync(id);
            if (assignment == null || assignment.UserId != userId) return false;
            var userCards = await _reviewStore.GetUserHand(userId);
            var card = userCards.Hand.FirstOrDefault(a => a.Assignment.Id == assignment.Id && a.FromField == fieldFrom && a.ToField == fieldTo);
            if (card == null) throw new InvalidOperationException("Trying to submit missing card.");
            var solution = assignment.ValueFromFieldType(fieldTo);
            var maxDifference = Math.Max(1, solution.Length / 4);
            if (fieldTo == ReviewField.Pronunciation || fieldTo == ReviewField.Reading) maxDifference = 0;
            var actualDifference = input.Levenshtein(solution);
            if (actualDifference > maxDifference)
            {
                await _reviewStore.MarkCardIncorrect(userId, card);
                return false;
            }
            await _reviewStore.DiscardUserCard(userId, card);
            if (userCards.Hand.All(a => a.Assignment.Id != assignment.Id) &&
                userCards.UpcomingQueue.All(a => a.Assignment.Id != assignment.Id))
            {
                return await CompleteReview(userId, assignment);
            }

            return true;
        }

        private async Task<bool> CompleteReview(string userId, [NotNull] Assignment assignment)
        {
            var cards = await _reviewStore.CleanAssignmentSet(userId, assignment);
            var wrongAnswers = cards.Select(a => a.IncorrectCount).Aggregate((a, b) => a + b);
            var review = new Review
            {
                UserId = userId,
                Lesson = assignment.Lesson,
                ReviewDone = DateTime.UtcNow,
                WrongAnswers = wrongAnswers,
                StartStage = -1,
                EndStage = 0
            };

            assignment.Stage = 0;
            assignment.NextReview = _scheduler.GetNextReview(assignment, 0);
            await _assignmentRepository.CreateOrUpdateAssignmentAsync(assignment);
            await _reviewRepository.CreateOrUpdateReviewAsync(review);

            return true;
        }
    }
}