using System.Threading.Tasks;
using MemoryApi.Core.DbModels;

namespace MemoryApi.Core.Database.Repositories
{
    public interface IThemeRepository
    {
        Task<Theme> GetUserThemeAsync(string userId);
    }
}