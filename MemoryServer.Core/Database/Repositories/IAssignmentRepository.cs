﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MemoryCore.DbModels;

namespace MemoryServer.Core.Database.Repositories
{
    public interface IAssignmentRepository
    {
        Task<List<LessonAssignment>> GetAssignmentsByUserAsync(User user);
        Task<LessonAssignment> FindAssignmentByIdAsync(Guid id);
        Task<LessonAssignment> FindAssignmentByLessonIdAsync(Guid lessonId);
        Task<int> DeleteAssignmentById(Guid assignmentId);
        Task<LessonAssignment> CreateOrUpdateAssignmentAsync(LessonAssignment assignment);
        Task<LessonAssignment> GetAssignmentByIdAsync(Guid id);
        Task<int> DeleteAssignmentAsync(LessonAssignment assignment);
        Task<int> CreateOrUpdateBurnedAssignmentAsync(BurnedAssignment burnedAssignment);
        Task<int> GetPendingLessonCountAsync(User user);
        Task<List<LessonAssignment>> GetPendingLessonsAsync(User user);
    }
}