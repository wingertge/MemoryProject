using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FaunaDB.Client;
using JetBrains.Annotations;
using MemoryCore.DbModels;
using MemoryServer.Core.Database.Repositories;

using static FaunaDB.Query.Language;

namespace MemoryApi.Functions.Database
{
    public class FaunaAssignmentRepository : IAssignmentRepository
    {
        private readonly FaunaClient _faunaClient;

        public FaunaAssignmentRepository([NotNull] SecretConfig secretConfig)
        {
            _faunaClient = new FaunaClient(secretConfig.FaunaSecret);
        }

        public Task<List<LessonAssignment>> GetAssignmentsByUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<LessonAssignment> FindAssignmentByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<LessonAssignment> FindAssignmentByLessonIdAsync(Guid lessonId)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAssignmentById(Guid assignmentId)
        {
            throw new NotImplementedException();
        }

        public Task<LessonAssignment> CreateOrUpdateAssignmentAsync(LessonAssignment assignment)
        {
            throw new NotImplementedException();
        }

        public Task<LessonAssignment> GetAssignmentByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAssignmentAsync(LessonAssignment assignment)
        {
            throw new NotImplementedException();
        }

        public Task<int> CreateOrUpdateBurnedAssignmentAsync(BurnedAssignment burnedAssignment)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetPendingLessonCountAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<List<LessonAssignment>> GetPendingLessonsAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}