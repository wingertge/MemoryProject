using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemoryCore;
using MemoryCore.DbModels;
using MemoryCore.Models;
using MemoryServer.Core.Business.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MemoryServer.Controllers
{
    [Route("api/[controller]"), Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        public AccountController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new MemoryCore.ActionResult
                {
                    Succeeded = false,
                    Errors = ModelState.ToErrorDictionary()
                });
            }

            var user = await _userManager.GetUserAsync(User);
            if (!await _userManager.CheckPasswordAsync(user, model.OldPassword))
            {
                ModelState.AddModelError("OldPassword", "Old password invalid.");
                return Json(new MemoryCore.ActionResult
                {
                    Succeeded = false,
                    Errors = ModelState.ToErrorDictionary()
                });
            }

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            return Json(new MemoryCore.ActionResult
            {
                Succeeded = result.Succeeded,
                Errors = result.Errors.GroupBy(a => a.Code).ToDictionary(a => a.Key, a => new List<string> { a.First().Description })
            });
        }
    }
}