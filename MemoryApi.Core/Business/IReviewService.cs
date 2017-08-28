using System;
using System.Threading.Tasks;
using MemoryCore.Models;

namespace MemoryApi.Core.Business
{
    public interface IReviewService
    {
        Task<int> GetPendingReviewCountAsync(string userId);
        Task<DateTime> GetNextReviewTimeAsync(string userId);
        Task<int> GetNextDayUpcomingReviewCountAsync(string userId);
        Task<ReviewModel> GetNextReviewAsync(string userId);
        Task<int> GetNextHourUpcomingReviewCountAsync(string userId);
        Task<bool> SubmitReviewAsync(string userId, Guid id, ReviewField fieldFrom, ReviewField fieldTo, string input);
    }
}