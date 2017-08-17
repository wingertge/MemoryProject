using System.Collections.Generic;
using System.Threading.Tasks;
using MemoryCore.DbModels;

namespace MemoryServer.Core.Database.Repositories
{
    public interface ILanguageRepository
    {
        Task<Language> FindLanguageByIdAsync(int id);
        Task<List<Language>> GetLanguageListByUserPopularityAsync(User user, bool from);
        Task<List<Language>> GetLanguageListByGlobalPopularityAsync(bool from);
        Task<List<Language>> GetLanguageListFull();
        Task<int> GetLanguageCount();
    }
}