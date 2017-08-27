using System;
using System.Threading.Tasks;
using MemoryApi.Core.Database.Repositories;
using MemoryApi.Core.DbModels;
using Microsoft.Azure.Documents.Client;

namespace MemoryApi.Functions.Database.Repositories.Impl
{
    public class DocumentDbThemeRepository : IThemeRepository
    {
        private readonly DocumentClient _db;
        private const string DatabaseId = Config.DatabaseId;
        private const string CollectionId = Config.CollectionId;
        private static readonly Uri Collection = UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId);

        public DocumentDbThemeRepository(DocumentClient db) => _db = db;

        public Task<Theme> GetUserThemeAsync(string userId)
        {
            return _db.CreateDocumentQuery<Theme>(Collection).FirstOrDefaultAsync(a => a.UserId == userId);
        }
    }
}