using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemoryCore.DbModels;
using Microsoft.EntityFrameworkCore;

namespace MemoryServer.Core.Database.Repositories.Impl
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ITransactionFactory<MemoryContext> _transactions;
        public ReviewRepository(ITransactionFactory<MemoryContext> transactions) => _transactions = transactions;

        public Task<int> GetPendingReviewsCountAsync(User user) => 
            _transactions.CreateAsyncTransaction(context => context.Assignments.Where(a => a.Owner.Id == user.Id && a.NextReview <= DateTime.Now).CountAsync()).Run();

        public Task<DateTime> GetLowestReviewTimeAsync(User user) => 
            _transactions.CreateAsyncTransaction(context => context.Assignments.Where(a => a.Owner.Id == user.Id && a.Stage != -1).MinAsync(a => a.NextReview)).Run();

        public Task<int> GetReviewCountFromToAsync(User user, DateTime @from, DateTime to) => 
            _transactions.CreateAsyncTransaction(context => context.Assignments.CountAsync(a => a.Owner.Id == user.Id && a.NextReview > @from && a.NextReview < to)).Run();

        public Task<List<LessonAssignment>> GetOldestReviews(User user, int limit) => 
            _transactions.CreateAsyncTransaction(context => context.Assignments.Where(a => a.Owner.Id == user.Id).Include(a => a.Lesson).OrderBy(a => a.NextReview).Take(limit).ToListAsync()).Run();

        public Task<Review> CreateOrUpdateReviewAsync(Review review)
        {
            return _transactions.CreateAsyncTransaction(async context =>
            {
                var result = review.Id != Guid.Empty ? context.Reviews.Update(review) : context.Reviews.Add(review);
                await context.SaveChangesAsync();
                return result.Entity;
            }).Run();
        }
    }
}