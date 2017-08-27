using System;
using System.Linq;
using JetBrains.Annotations;
using MemoryApi.Core.DbModels;
using Microsoft.Azure.Documents.Client;

namespace MemoryApi.Functions.Extensions
{
    public static class DocumentDbExtensions
    {
        [NotNull]
        public static IQueryable<T> CreateEntityQuery<T>([NotNull] this DocumentClient db, Uri collectionPath, FeedOptions options = null) where T : DbModel
        {
            return db.CreateDocumentQuery<T>(collectionPath, options).Where(a => a.Entity == typeof(T).Name);
        }
    }
}