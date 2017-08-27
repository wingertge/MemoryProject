using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Azure.Documents.Linq;

namespace MemoryApi.Functions.Database
{
    public static class QueryableExtensions
    {
        [ItemNotNull]
        public static async Task<List<T>> ToListAsync<T>(this IQueryable<T> query)
        {
            return (await query.AsDocumentQuery().ExecuteNextAsync<T>()).ToList();
        }

        [ItemCanBeNull]
        public static async Task<T> FirstOrDefaultAsync<T>(this IQueryable<T> query, [CanBeNull] Expression<Func<T, bool>> selector = null)
        {
            return selector != null ? (await query.Where(selector).AsDocumentQuery().ExecuteNextAsync<T>()).FirstOrDefault() : (await query.AsDocumentQuery().ExecuteNextAsync<T>()).FirstOrDefault();
        }
    }
}