using System.Collections.Generic;
using System.Threading.Tasks;
using MemoryApi.Core.DbModels;

namespace MemoryApi.Core.Business
{
    public interface IAutocompleteService
    {
        Task<List<string>> GetAutocompleteResults<TModel>(string query, string fieldSelector, string conditionals) where TModel : DbModel;
    }
}