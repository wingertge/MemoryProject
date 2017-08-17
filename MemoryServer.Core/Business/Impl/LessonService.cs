using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemoryCore.DbModels;
using MemoryCore.Models;
using MemoryServer.Core.Business.Util;
using MemoryServer.Core.Database.Repositories;

namespace MemoryServer.Core.Business.Impl
{
    public class LessonService : ILessonService
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IScheduler _scheduler;
        private readonly ILessonReviewStore _reviewStore;
        private readonly IReviewRepository _reviewRepository;

        private readonly Random _random = new Random();

        public LessonService(IAssignmentRepository assignmentRepository, ILessonRepository lessonRepository, 
            ILanguageRepository languageRepository, IUserRepository userRepository, IScheduler scheduler,
            ILessonReviewStore reviewStore, IReviewRepository reviewRepository)
        {
            _assignmentRepository = assignmentRepository;
            _lessonRepository = lessonRepository;
            _languageRepository = languageRepository;
            _userRepository = userRepository;
            _scheduler = scheduler;
            _reviewStore = reviewStore;
            _reviewRepository = reviewRepository;
        }

        public async Task<List<LessonAssignment>> GetUserAssignments(User user) => await _assignmentRepository.GetAssignmentsByUserAsync(user);

        public async Task<LessonAssignment> CreateOrEditAssignment(int langFromId, int langToId, string reading, string pronunciation, string meaning, User creator, Guid assignmentId)
        {
            var existingLesson = await _lessonRepository.FindLessonByContentAsync(langFromId, langToId, reading, pronunciation, meaning);
            var langFrom = await _languageRepository.FindLanguageByIdAsync(langFromId);
            var langTo = await _languageRepository.FindLanguageByIdAsync(langToId);
            if (existingLesson == null)
            {
                var newLesson = new Lesson
                {
                    LanguageFrom = langFrom,
                    LanguageTo = langTo,
                    Reading = reading,
                    Pronunciation = pronunciation,
                    Meaning = meaning
                };
                existingLesson = await _lessonRepository.CreateOrUpdateLessonAsync(newLesson);
            }

            var assignment = await _assignmentRepository.FindAssignmentByLessonIdAsync(existingLesson.Id);
            if (assignment != null)
            {
                if (assignmentId != Guid.Empty) await _assignmentRepository.DeleteAssignmentById(assignmentId);
                return null;
            }

            if (assignmentId != Guid.Empty) assignment = await _assignmentRepository.FindAssignmentByIdAsync(assignmentId);
            else
            {
                assignment = new LessonAssignment
                {
                    Owner = creator,
                    Stage = -1
                };
            }

            assignment.Lesson = existingLesson;
            assignment.NextReview = _scheduler.GetNextReview(assignment, assignment.Stage);
            assignment = await _assignmentRepository.CreateOrUpdateAssignmentAsync(assignment);
            creator.LastLanguageFrom = langFrom;
            creator.LastLanguageTo = langTo;
            await _userRepository.CreateOrUpdateUserAsync(creator);
            
            return assignment;
        }

        public Task<int> GetPendingLessonReviewCount(User user) => _assignmentRepository.GetPendingLessonCountAsync(user);

        private async Task InitialiseHand(User user)
        {
            var assignments = await _assignmentRepository.GetPendingLessonsAsync(user);
            var context = new CardContext();
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
            await _reviewStore.InitaliseUserHand(user, context);
        }

        public async Task<ReviewModel> GetNextReviewAsync(User user)
        {
            var pendingReviews = await GetPendingLessonReviewCount(user);
            if (pendingReviews == 0) return null;
            if (!await _reviewStore.DoesUserExist(user))
                await InitialiseHand(user);

            var currentHand = await _reviewStore.GetUserHand(user);
            if (currentHand.Hand.Count == 0)
            {
                await InitialiseHand(user);
                currentHand = await _reviewStore.GetUserHand(user);
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

        public async Task<bool> SubmitReviewAsync(User user, Guid id, ReviewField fieldFrom, ReviewField fieldTo, string input)
        {
            if (!await _reviewStore.DoesUserExist(user)) return false;
            var assignment = await _assignmentRepository.GetAssignmentByIdAsync(id);
            if (assignment == null || assignment.Owner != user) return false;
            var userCards = await _reviewStore.GetUserHand(user);
            var card = userCards.Hand.FirstOrDefault(a => a.Assignment.Id == assignment.Id && a.FromField == fieldFrom && a.ToField == fieldTo);
            if (card == null) throw new InvalidOperationException("Trying to submit missing card.");
            var solution = assignment.ValueFromFieldType(fieldTo);
            var maxDifference = Math.Max(1, solution.Length / 4);
            if (fieldTo == ReviewField.Pronunciation || fieldTo == ReviewField.Reading) maxDifference = 0;
            var actualDifference = input.Levenshtein(solution);
            if (actualDifference > maxDifference)
            {
                await _reviewStore.MarkCardIncorrect(user, card);
                return false;
            }
            await _reviewStore.DiscardUserCard(user, card);
            if (userCards.Hand.All(a => a.Assignment.Id != assignment.Id) &&
                userCards.UpcomingQueue.All(a => a.Assignment.Id != assignment.Id))
            {
                return await CompleteReview(user, assignment);
            }

            return true;
        }

        private async Task<bool> CompleteReview(User user, LessonAssignment assignment)
        {
            var cards = await _reviewStore.CleanAssignmentSet(user, assignment);
            var wrongAnswers = cards.Select(a => a.IncorrectCount).Aggregate((a, b) => a + b);
            var review = new Review
            {
                Lesson = assignment,
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