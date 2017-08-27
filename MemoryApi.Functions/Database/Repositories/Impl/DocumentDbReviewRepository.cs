using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MemoryApi.Core.Database.Repositories;
using MemoryApi.Core.DbModels;
using MemoryApi.Functions.Extensions;
using MemoryServer.Core;
using Microsoft.Azure.Documents.Client;

namespace MemoryApi.Functions.Database.Repositories.Impl
{
    public class DocumentDbReviewRepository : IReviewRepository
    {
        private readonly DocumentClient _db;
        private const string DatabaseId = Config.DatabaseId;
        private const string CollectionId = Config.CollectionId;
        private static readonly Uri Collection = UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId);

        public DocumentDbReviewRepository(DocumentClient db) => _db = db;

        [NotNull]
        public Task<int> GetPendingReviewsCountAsync(string userId)
        {
            return _db.CreateDocumentQuery<int>(Collection,
                    $"select value count(a) from root as a where a.Entity = 'Assignment' and a.UserId = '{userId}' and a.Burned = false and a.Active = true and a.NextReview < {DateTime.UtcNow.ToUnixMillis()}")
                .FirstOrDefaultAsync();
        }

        [NotNull]
        public Task<long> GetLowestReviewTimeAsync(string userId)
        {
            return _db.CreateDocumentQuery<long>(Collection,
                    $"select value min(a.NextReview) from root as a where a.Entity = 'Assignment' and a.UserId = '{userId}' and a.Burned = false and a.Active = true")
                .FirstOrDefaultAsync();
        }

        [NotNull]
        public Task<int> GetReviewCountFromToAsync(string userId, DateTime @from, DateTime to)
        {
            return _db.CreateDocumentQuery<int>(Collection,
                    $"select value count(a) from root as a where a.Entity = 'Assignment' and a.UserId = '{userId}' and a.NextReview between {@from.ToUnixMillis()} and {to.ToUnixMillis()}")
                .FirstOrDefaultAsync();
        }

        public Task<List<Assignment>> GetOldestReviews(string userId, int limit)
        {
            return _db.CreateEntityQuery<Assignment>(Collection).Where(a => a.UserId == userId)
                .OrderBy(a => a.NextReview).Take(limit).ToListAsync();
        }

        public async Task<Review> CreateOrUpdateReviewAsync(Review review)
        {
            return (Review) await _db.UpsertDocumentAsync(Collection, review);
        }
    }
}