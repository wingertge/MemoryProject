using System.Threading.Tasks;
using MemoryApi.Core.Business;
using MemoryCore.DbModels;
using MemoryServer.Core.Business;
using MemoryServer.Core.Business.Util;
using MemoryServer.Core.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MemoryServer.Controllers
{
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly RoleManager<DummyRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AdminController> _logger;

        public AdminController(RoleManager<DummyRole> roleManager, UserManager<User> userManager, ILogger<AdminController> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet("roles-init"), AllowAnonymous]
        public async Task<IActionResult> CreateRolesAndUsers()
        {
            var exists = await _roleManager.RoleExistsAsync(Roles.Admin);
            if (exists) return BadRequest();

            var role = new DummyRole {Name = Roles.Admin};
            await _roleManager.CreateAsync(role);

            var user = new User
            {
                UserName = "admin",
                Email = "admin@project-memory.org"
            };

            var pwd = PasswordGenerator.GenerateRandomString(12);
            _logger.LogWarning($"Generated password for default user: {pwd}");

            var chkUser = await _userManager.CreateAsync(user, pwd);

            if (chkUser.Succeeded) await _userManager.AddToRoleAsync(user, Roles.Admin);

            return Ok();
        }
    }
}