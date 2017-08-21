using System.Linq;
using System.Threading.Tasks;
using MemoryCore;
using MemoryCore.DbModels;
using MemoryCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MemoryServer.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        public AccountController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new MemoryCore.ActionResult
                {
                    Succeeded = false,
                    Errors = ModelState.SelectMany(a => a.Value.Errors.ToList().Select(b => (a.Key, b.ErrorMessage)))
                        .ToList()
                });
            }

            var user = await _userManager.GetUserAsync(User);
            if (!await _userManager.CheckPasswordAsync(user, model.OldPassword))
            {
                ModelState.AddModelError("OldPassword", "Old password invalid.");
                return Json(new MemoryCore.ActionResult
                {
                    Succeeded = false,
                    Errors = ModelState.SelectMany(a => a.Value.Errors.ToList().Select(b => (a.Key, b.ErrorMessage)))
                        .ToList()
                });
            }

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            return Json(new MemoryCore.ActionResult
            {
                Succeeded = result.Succeeded,
                Errors = result.Errors.Select(a => (a.Code, a.Description)).ToList()
            });
        }
    }
}