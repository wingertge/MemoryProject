using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MemoryApi.Core.DbModels;
using MemoryCore.Models;

namespace MemoryApi.Core.Business
{
    public interface IAssignmentService
    {
        Task<List<Assignment>> GetUserAssignments(string userId);
        Task<Assignment> CreateOrEditAssignment(int langFromId, int langToId, string reading, string pronunciation, string meaning, string creatorId, Guid assignmentId);
        Task<int> GetPendingLessonReviewCount(string userId);
        Task<ReviewModel> GetNextReviewAsync(string userId);
        Task<bool> SubmitReviewAsync(string userId, Guid id, ReviewField fieldFrom, ReviewField fieldTo, string input);
    }
}