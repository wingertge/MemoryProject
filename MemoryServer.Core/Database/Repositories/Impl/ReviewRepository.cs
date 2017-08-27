using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemoryApi.Core.Database.Repositories;
using MemoryCore.DbModels;
using Microsoft.EntityFrameworkCore;

namespace MemoryServer.Core.Database.Repositories.Impl
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ITransactionFactory<MemoryContext> _transactions;
        public ReviewRepository(ITransactionFactory<MemoryContext> transactions) => _transactions = transactions;

        public Task<int> GetPendingReviewsCountAsync(string userId) => 
            _transactions.CreateAsyncTransaction(context => context.Assignments.Where(a => a.OwnerId == userId && a.NextReview <= DateTime.Now).CountAsync()).Run();

        public Task<DateTime> GetLowestReviewTimeAsync(string userId) => 
            _transactions.CreateAsyncTransaction(context => context.Assignments.Where(a => a.OwnerId == userId && a.Stage != -1).MinAsync(a => a.NextReview)).Run();

        public Task<int> GetReviewCountFromToAsync(string userId, DateTime @from, DateTime to) => 
            _transactions.CreateAsyncTransaction(context => context.Assignments.CountAsync(a => a.OwnerId == userId && a.NextReview > @from && a.NextReview < to)).Run();

        public Task<List<LessonAssignment>> GetOldestReviews(string userId, int limit) => 
            _transactions.CreateAsyncTransaction(context => context.Assignments.Where(a => a.OwnerId == userId).Include(a => a.Lesson).OrderBy(a => a.NextReview).Take(limit).ToListAsync()).Run();

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