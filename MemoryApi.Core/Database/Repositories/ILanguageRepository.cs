using System.Collections.Generic;
using System.Threading.Tasks;
using MemoryCore.DataTypes;

namespace MemoryApi.Core.Database.Repositories
{
    public interface ILanguageRepository
    {
        Task<Language> FindLanguageByIdAsync(int id);
        Task<List<Language>> GetLanguageListByUserPopularityAsync(string userId, bool from);
        Task<List<Language>> GetLanguageListByGlobalPopularityAsync(bool from);
        Task<List<Language>> GetLanguageListFull();
        Task<int> GetLanguageCount();
    }
}