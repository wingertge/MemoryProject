using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MemoryApi.Core.DbModels;

namespace MemoryApi.Core.Database.Repositories
{
    public interface IReviewRepository
    {
        Task<int> GetPendingReviewsCountAsync(string userId);
        Task<long> GetLowestReviewTimeAsync(string userId);
        Task<int> GetReviewCountFromToAsync(string userId, DateTime @from, DateTime to);
        Task<List<Assignment>> GetOldestReviews(string userId, int limit);
        Task<Review> CreateOrUpdateReviewAsync(Review review);
    }
}