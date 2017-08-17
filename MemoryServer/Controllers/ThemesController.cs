using System.Threading.Tasks;
using MemoryCore;
using MemoryCore.DbModels;
using MemoryServer.Core.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ActionResult = Microsoft.AspNetCore.Mvc.ActionResult;

namespace MemoryServer.Controllers
{
    [Authorize, Route("api/[controller]")]
    public class ThemesController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IThemeService _themeService;

        public ThemesController(UserManager<User> userManager, IThemeService themeService)
        {
            _userManager = userManager;
            _themeService = themeService;
        }

        [HttpGet("current-theme")]
        public async Task<ActionResult> CurrentTheme()
        {
            var user = await _userManager.GetUserAsync(User);
            return Json(new JsonResult<Theme>(_themeService.GetCurrentTheme(user)));
        }
    }
}