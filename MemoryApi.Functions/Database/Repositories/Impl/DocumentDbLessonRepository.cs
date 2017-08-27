using System;
using System.Threading.Tasks;
using MemoryApi.Core.Database.Repositories;
using MemoryApi.Core.DbModels;
using MemoryApi.Functions.Extensions;
using Microsoft.Azure.Documents.Client;

namespace MemoryApi.Functions.Database.Repositories.Impl
{
    public class DocumentDbLessonRepository : ILessonRepository
    {
        private readonly DocumentClient _db;
        private const string DatabaseId = Config.DatabaseId;
        private const string CollectionId = Config.CollectionId;
        private static readonly Uri Collection = UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId);

        public DocumentDbLessonRepository(DocumentClient db) => _db = db;

        public Task<Lesson> FindLessonByContentAsync(int langFrom, int langTo, string reading, string pronunciation, string meaning)
        {
            return _db.CreateEntityQuery<Lesson>(Collection).FirstOrDefaultAsync(a =>
                a.Languages.LanguageFromId == langFrom && a.Languages.LanguageToId == langTo && a.Reading == reading &&
                a.Pronunciation == pronunciation && a.Meaning == meaning);
        }

        public async Task<Lesson> CreateOrUpdateLessonAsync(Lesson lesson) => (Lesson) await _db.UpsertDocumentAsync(Collection, lesson);
    }
}