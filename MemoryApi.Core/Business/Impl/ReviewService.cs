using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MemoryApi.Core.Database.Repositories;
using MemoryApi.Core.DbModels;
using MemoryCore.Models;
using MemoryServer.Core.Business.Util;

namespace MemoryApi.Core.Business.Impl
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IReviewStore _reviewStore;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IScheduler _scheduler;
        private readonly Random _random = new Random();

        public ReviewService(IReviewRepository reviewRepository, IReviewStore reviewStore, IAssignmentRepository assignmentRepository, IScheduler scheduler)
        {
            _reviewRepository = reviewRepository;
            _reviewStore = reviewStore;
            _assignmentRepository = assignmentRepository;
            _scheduler = scheduler;
        }

        public Task<int> GetPendingReviewCountAsync(string userId) => _reviewRepository.GetPendingReviewsCountAsync(userId);

        public Task<DateTime> GetNextReviewTimeAsync(string userId) => _reviewRepository.GetLowestReviewTimeAsync(userId);
        
        public Task<int> GetNextDayUpcomingReviewCountAsync(string userId) => _reviewRepository.GetReviewCountFromToAsync(userId, DateTime.UtcNow, DateTime.UtcNow + TimeSpan.FromDays(1));

        public Task<int> GetNextHourUpcomingReviewCountAsync(string userId) => _reviewRepository.GetReviewCountFromToAsync(userId, DateTime.UtcNow, DateTime.UtcNow + TimeSpan.FromHours(1));

        private async Task InitialiseHand(string userId, int count)
        {
            var assignments = await _reviewRepository.GetOldestReviews(userId, count);
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
            
            context.FillHand(20);
            await _reviewStore.InitaliseUserHand(userId, context);
        }

        public async Task<ReviewModel> GetNextReviewAsync(string userId)
        {
            var pendingReviews = await _reviewRepository.GetPendingReviewsCountAsync(userId);
            if (pendingReviews == 0) return null;
            if (!await _reviewStore.DoesUserExist(userId))
                await InitialiseHand(userId, pendingReviews);

            var currentHand = await _reviewStore.GetUserHand(userId);
            if (currentHand.Hand.Count == 0)
            {
                await InitialiseHand(userId, pendingReviews);
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
            if(card == null) throw new InvalidOperationException("Trying to submit missing card.");
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
                StartStage = assignment.Stage
            };

            if (wrongAnswers > 0)
            {
                var newStage = Math.Max(assignment.Stage, 0);
                review.EndStage = newStage;
                assignment.Stage = newStage;
                assignment.NextReview = _scheduler.GetNextReview(assignment, newStage);
            }
            else
            {
                var newStage = assignment.Stage++;
                if (newStage > 10)
                {
                    assignment.Burned = true;
                    return true;
                }

                review.EndStage = newStage;
                assignment.Stage = newStage;
                assignment.NextReview = _scheduler.GetNextReview(assignment, newStage);
            }

            await _assignmentRepository.CreateOrUpdateAssignmentAsync(assignment);
            await _reviewRepository.CreateOrUpdateReviewAsync(review);
            return true;
        }
    }
}