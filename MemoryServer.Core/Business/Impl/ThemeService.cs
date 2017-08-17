using System.Threading.Tasks;
using MemoryCore.DbModels;

namespace MemoryServer.Core.Business.Impl
{
    public class ThemeService : IThemeService
    {
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

        public Theme GetCurrentTheme(User user)
        {
            return user.Theme ?? DefaultTheme;
        }
    }
}