using System;
using System.Threading.Tasks;
using MemoryCore.DbModels;
using MemoryCore.Models;

namespace MemoryServer.Core.Business
{
    public interface IReviewService
    {
        Task<int> GetPendingReviewCountAsync(User user);
        Task<DateTime> GetNextReviewTimeAsync(User user);
        Task<int> GetNextDayUpcomingReviewCountAsync(User user);
        Task<ReviewModel> GetNextReviewAsync(User user);
        Task<int> GetNextHourUpcomingReviewCountAsync(User user);
        Task<bool> SubmitReviewAsync(User user, Guid id, ReviewField fieldFrom, ReviewField fieldTo, string input);
    }
}