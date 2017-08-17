using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MemoryServer.Core.Database;
using Microsoft.EntityFrameworkCore;

namespace MemoryServer.Core.Business
{
    public interface IAutocompleteService
    {
        Task<List<string>> GetAutocompleteResults<TModel>(string query, Func<MemoryContext, DbSet<TModel>> setSelector, Expression<Func<TModel, string>> fieldSelector, params Expression<Func<TModel, bool>>[] conditonals) where TModel : class;
    }
}