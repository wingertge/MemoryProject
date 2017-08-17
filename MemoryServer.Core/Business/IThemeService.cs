using System.Threading.Tasks;
using MemoryCore.DbModels;

namespace MemoryServer.Core.Business
{
    public interface IThemeService
    {
        Theme GetCurrentTheme(User user);
    }
}