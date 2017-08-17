using System.Threading.Tasks;
using MemoryCore;
using MemoryCore.DbModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MemoryServer.Controllers
{
    [Authorize, Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        public UsersController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("current")]
        public async Task<JsonResult> Current()
        {
            var user = await _userManager.GetUserAsync(User);
            return Json(new JsonResult<User>(user));
        }
    }
}