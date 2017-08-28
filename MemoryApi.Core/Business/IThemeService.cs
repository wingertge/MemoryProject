using System.Threading.Tasks;
using MemoryApi.Core.DbModels;

namespace MemoryApi.Core.Business
{
    public interface IThemeService
    {
        Task<Theme> GetCurrentTheme(string userId);
    }
}