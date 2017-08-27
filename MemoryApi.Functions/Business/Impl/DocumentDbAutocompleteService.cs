using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemoryApi.Core.Business;
using MemoryApi.Core.DbModels;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace MemoryApi.Functions.Business.Impl
{
    public class DocumentDbAutocompleteService : IAutocompleteService
    {
        private const string DatabaseId = Config.DatabaseId;
        private const string CollectionId = Config.CollectionId;
        private readonly DocumentClient _db;
        private static readonly Uri Collection = UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId);

        public DocumentDbAutocompleteService(DocumentClient db)
        {
            _db = db;
        }

        public Task<List<string>> GetAutocompleteResults<TModel>(string queryString, string fieldSelector, string conditionals) where TModel : DbModel
        {
            var sql = $"select c.{fieldSelector} from root as c where c.Entity = '{typeof(TModel).Name}' and {conditionals} order by udf.levenshtein(c.{fieldSelector}, @query) asc";
            var compiledSql = new SqlQuerySpec
            {
                QueryText = sql,
                Parameters = new SqlParameterCollection {new SqlParameter("@query", queryString)}
            };
            var query = _db.CreateDocumentQuery<string>(Collection, compiledSql, new FeedOptions { MaxItemCount = 5 });
            return query.ToAsyncEnumerable().ToList();
        }
    }
}