using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MemoryApi.Core.Database.Repositories;
using MemoryApi.Core.DbModels;
using MemoryApi.Functions.Extensions;
using Microsoft.Azure.Documents.Client;

namespace MemoryApi.Functions.Database.Repositories.Impl
{
    public class DocumentDbAssignmentRepository : IAssignmentRepository
    {
        private const string DatabaseId = Config.DatabaseId;
        private const string CollectionId = Config.CollectionId;
        private readonly DocumentClient _db;
        private static readonly Uri Collection = UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId);

        public DocumentDbAssignmentRepository(DocumentClient db)
        {
            _db = db;
        }

        public Task<List<Assignment>> GetAssignmentsByUserAsync(string userId)
        {
            return _db.CreateEntityQuery<Assignment>(Collection).Where(a => a.UserId == userId).ToListAsync();
        }

        public Task<Assignment> FindAssignmentByIdAsync(string id)
        {
            return _db.CreateEntityQuery<Assignment>(Collection).FirstOrDefaultAsync(a => a.Id == id);
        }

        public Task<List<Assignment>> FindAssignmentsByLessonIdAsync(string lessonId)
        {
            return _db.CreateEntityQuery<Assignment>(Collection).Where(a => a.Lesson.Id == lessonId).ToListAsync();
        }

        public async Task<int> DeleteAssignmentByIdAsync(string assignmentId)
        {
            var result = await _db.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, assignmentId));
            return (int) result.StatusCode < 400 ? 0 : 1;
        }

        public async Task<Assignment> CreateOrUpdateAssignmentAsync(Assignment assignment)
        {
            var response = await _db.UpsertDocumentAsync(Collection, assignment);
            return (Assignment)response.Resource;
        }

        public async Task<Assignment> GetAssignmentByIdAsync(string id)
        {
            return (dynamic) await _db.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
        }

        [NotNull]
        public Task<int> DeleteAssignmentAsync([NotNull] Assignment assignment)
        {
            return DeleteAssignmentByIdAsync(assignment.Id);
        }

        public Task<int> GetPendingLessonCountAsync(string userId)
        {
            return _db.CreateDocumentQuery<int>(Collection,
                    $"select value count(a) from root as a where a.Entity = Assignment and a.UserId = '{userId}' and a.Active = false").ToAsyncEnumerable()
                .First();
        }

        public Task<List<Assignment>> GetPendingLessonsAsync(string userId)
        {
            return _db.CreateEntityQuery<Assignment>(Collection)
                .Where(a => !a.Burned && !a.Active).ToListAsync();
        }

        public Task<Assignment> FindAssignmentByLessonIdAsync(string creatorId, string existingLessonId)
        {
            return _db.CreateEntityQuery<Assignment>(Collection)
                .FirstOrDefaultAsync(a => a.UserId == creatorId && a.Lesson.Id == existingLessonId);
        }
    }
}