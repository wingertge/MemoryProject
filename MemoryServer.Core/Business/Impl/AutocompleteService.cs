using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MemoryServer.Core.Database;
using Microsoft.EntityFrameworkCore;

namespace MemoryServer.Core.Business.Impl
{
    public class AutocompleteService : IAutocompleteService
    {
        private readonly ITransactionFactory<MemoryContext> _transactions;
        public AutocompleteService(ITransactionFactory<MemoryContext> transactions)
        {
            _transactions = transactions;
        }

        public Task<List<string>> GetAutocompleteResults<TModel>(string query, Func<MemoryContext, DbSet<TModel>> setSelector, Expression<Func<TModel, string>> fieldSelector, params Expression<Func<TModel, bool>>[] conditonals) where TModel : class
        {
            return _transactions.CreateAsyncTransaction(context =>
            {
                var store = setSelector(context);
                var subSet = conditonals.Aggregate<Expression<Func<TModel, bool>>, IQueryable<TModel>>(store, (current, conditonal) => current.Where(conditonal));
                var selectedValues = subSet.Select(fieldSelector);
                return selectedValues.OrderBy(a => MemoryContext.Levenshtein(a, query, 4000)).Take(5).ToListAsync();
            }).Run();
        }
    }
}