using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MemoryCore.DbModels;
using MemoryCore.Models;

namespace MemoryServer.Core.Business
{
    /* Lots of overlap between assignments and lessons, combining service */
    public interface ILessonService
    {
        Task<List<LessonAssignment>> GetUserAssignments(User user);
        Task<LessonAssignment> CreateOrEditAssignment(int langFromId, int langToId, string reading, string pronunciation, string meaning, User creator, Guid assignmentId);
        Task<int> GetPendingLessonReviewCount(User user);
        Task<ReviewModel> GetNextReviewAsync(User user);
        Task<bool> SubmitReviewAsync(User user, Guid id, ReviewField fieldFrom, ReviewField fieldTo, string input);
    }
}