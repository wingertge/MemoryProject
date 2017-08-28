using System.Collections.Generic;
using System.Threading.Tasks;
using MemoryApi.Core.DbModels;

namespace MemoryApi.Core.Database.Repositories
{
    public interface IAssignmentRepository
    {
        Task<List<Assignment>> GetAssignmentsByUserAsync(string userId);
        Task<Assignment> FindAssignmentByIdAsync(string id);
        Task<int> DeleteAssignmentByIdAsync(string assignmentId);
        Task<Assignment> CreateOrUpdateAssignmentAsync(Assignment assignment);
        Task<Assignment> GetAssignmentByIdAsync(string id);
        Task<int> DeleteAssignmentAsync(Assignment assignment);
        Task<int> GetPendingLessonCountAsync(string userId);
        Task<List<Assignment>> GetPendingLessonsAsync(string userId);
        Task<Assignment> FindAssignmentByLessonIdAsync(string creatorId, string existingLessonId);
    }
}