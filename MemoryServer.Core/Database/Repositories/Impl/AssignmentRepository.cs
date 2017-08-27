using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemoryApi.Core.Database.Repositories;
using MemoryCore.DbModels;
using Microsoft.EntityFrameworkCore;

namespace MemoryServer.Core.Database.Repositories.Impl
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly ITransactionFactory<MemoryContext> _db;
        public AssignmentRepository(ITransactionFactory<MemoryContext> db) => _db = db;

        public async Task<List<LessonAssignment>> GetAssignmentsByUserAsync(string userId)
        {
            var transaction = _db.CreateAsyncTransaction(context =>
            {
                return context.Assignments.Where(a => a.OwnerId == userId)
                    .Include(a => a.Lesson).ThenInclude(a => a.Audio)
                    .Include(a => a.Lesson.LanguageFrom)
                    .Include(a => a.Lesson.LanguageTo)
                    .OrderBy(a => a.Lesson.Reading)
                    .ToListAsync();
            });
            return await transaction.Run();
        }

        public Task<LessonAssignment> FindAssignmentByIdAsync(Guid id)
        {
            return _db.CreateAsyncTransaction(context => context.Assignments.FindAsync(id)).Run();
        }

        public Task<LessonAssignment> FindAssignmentByLessonIdAsync(Guid lessonId)
        {
            return _db.CreateAsyncTransaction(context =>
                context.Assignments.FirstOrDefaultAsync(a => a.Lesson.Id == lessonId)).Run();
        }

        public Task<int> DeleteAssignmentById(Guid assignmentId)
        {
            return _db.CreateAsyncTransaction(async context =>
            {
                var assignment = await context.Assignments.FindAsync(assignmentId);
                if (assignment != null) context.Assignments.Remove(assignment);
                return await context.SaveChangesAsync();
            }).Run();
        }

        public Task<LessonAssignment> CreateOrUpdateAssignmentAsync(LessonAssignment assignment)
        {
            return _db.CreateAsyncTransaction(async context =>
            {
                var result = assignment.Id == Guid.Empty ? context.Assignments.Add(assignment).Entity : context.Assignments.Update(assignment).Entity;
                await context.SaveChangesAsync();
                return result;
            }).Run();
        }

        public Task<LessonAssignment> GetAssignmentByIdAsync(Guid id)
        {
            return _db.CreateAsyncTransaction(context => context.Assignments.Include(a => a.Lesson).SingleOrDefaultAsync(a => a.Id == id)).Run();
        }

        public Task<int> DeleteAssignmentAsync(LessonAssignment assignment)
        {
            return _db.CreateAsyncTransaction(context =>
            {
                context.Assignments.Remove(assignment);
                return context.SaveChangesAsync();
            }).Run();
        }

        public Task<int> CreateOrUpdateBurnedAssignmentAsync(BurnedAssignment burnedAssignment)
        {
            return _db.CreateAsyncTransaction(context =>
            {
                context.BurnedAssignments.Add(burnedAssignment);
                return context.SaveChangesAsync();
            }).Run();
        }

        public Task<int> GetPendingLessonCountAsync(string userId)
        {
            return _db.CreateAsyncTransaction(async context =>
                await context.Assignments.CountAsync(a => a.OwnerId == userId && a.Stage == -1) +
                await context.ListUpdateTrackers.CountAsync(a => a.UserId == userId)).Run();
        }

        public Task<List<LessonAssignment>> GetPendingLessonsAsync(string userId)
        {
            return _db.CreateAsyncTransaction(async context =>
            {
                var l1 = await context.Assignments.Where(a => a.OwnerId == userId && a.Stage == -1).ToListAsync();
                var l2 = await context.ListUpdateTrackers.Where(a => a.UserId == userId).ToListAsync();
                var assignments2 = l2.Select(a => new LessonAssignment
                {
                    Lesson = a.Entry.Lesson,
                    NextReview = DateTime.MaxValue,
                    OwnerId = a.UserId,
                    Stage = -1
                }).ToList();
                return l1.Concat(assignments2).ToList();
            }).Run();
        }
    }
}