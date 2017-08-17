using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MemoryCore.DbModels;

namespace MemoryServer.Core.Database.Repositories
{
    public interface IReviewRepository
    {
        Task<int> GetPendingReviewsCountAsync(User user);
        Task<DateTime> GetLowestReviewTimeAsync(User user);
        Task<int> GetReviewCountFromToAsync(User user, DateTime @from, DateTime to);
        Task<List<LessonAssignment>> GetOldestReviews(User user, int limit);
        Task<Review> CreateOrUpdateReviewAsync(Review review);
    }
}