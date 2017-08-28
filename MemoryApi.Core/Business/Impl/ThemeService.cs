using System.Threading.Tasks;
using MemoryApi.Core.Database.Repositories;
using MemoryApi.Core.DbModels;

namespace MemoryApi.Core.Business.Impl
{
    public class ThemeService : IThemeService
    {
        private readonly IThemeRepository _themeRepository;

        private static readonly Theme DefaultTheme = new Theme
        {
            BackgroundPrimary = "#FFC0CB",
            BackgroundSecondary = "white",
            BackgroundTertiary = "black",

            BorderPrimary = "#ff97a9",
            BorderSecondary = "black",
            BorderTertiary = "white",

            TextPrimary = "#464a4c",
            TextSecondary = "black",
            TextTertiary = "white"
        };

        public ThemeService(IThemeRepository themeRepository)
        {
            _themeRepository = themeRepository;
        }

        public Task<Theme> GetCurrentTheme(string userId)
        {
            return _themeRepository.GetUserThemeAsync(userId);
        }
    }
}