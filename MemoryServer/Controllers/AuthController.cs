using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MemoryCore;
using MemoryCore.DbModels;
using MemoryCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ActionResult = MemoryCore.ActionResult;

namespace MemoryServer.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger _logger;
        private readonly IStringLocalizer _localizer;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AuthController> logger, IStringLocalizer<AuthController> localizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _localizer = localizer;
        }

        [HttpPost("login"), AllowAnonymous]
        public async Task<JsonResult> Login([FromBody]LoginModel model)
        {
            if (model.Identifier.IndexOf('@') > -1)
            {
                //Validate email format
                const string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                          @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                          @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                var re = new Regex(emailRegex);
                if (!re.IsMatch(model.Identifier))
                {
                    ModelState.AddModelError("Identifier", "Email is not valid");
                }
            }
            else
            {
                //validate Username format
                const string emailRegex = @"^[a-zA-Z0-9]*$";
                var re = new Regex(emailRegex);
                if (!re.IsMatch(model.Identifier))
                {
                    ModelState.AddModelError("Identifier", "Username is not valid");
                }
            }

            var username = model.Identifier;
            if (username.Contains("@"))
            {
                var user = await _userManager.FindByEmailAsync(username);
                if (user == null)
                {
                    ModelState.AddModelError("", _localizer["InvalidCredentials"]);
                    return Json(new ActionResult
                    {
                        Succeeded = false,
                        Errors = ModelState.Select(a => (ActionErrors) new ModelActionErrors(a.Key, a.Value.Errors))
                            .ToList(),
                        ErrorCode = (int) ErrorCodes.InvalidCredentials
                    });
                }
                username = user.UserName;
            }
            var result = await _signInManager.PasswordSignInAsync(username, model.Password, model.Remember,
                lockoutOnFailure: false);
            var actionResult = new ActionResult { Succeeded = result.Succeeded };
            if (result.Succeeded)
            {
                _logger.LogInformation((int)ErrorCodes.Success, "User logged in.");
                return Json(actionResult);
            }

            if (result.RequiresTwoFactor)
            {
                actionResult.ErrorCode = (int)ErrorCodes.RequiresTwoFactor;
                actionResult.Error = _localizer["TwoFactor"];
                return Json(actionResult);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning((int) ErrorCodes.LockedOut, "User account locked out.");
                actionResult.Error = _localizer["LockedOut"];
                actionResult.ErrorCode = (int) ErrorCodes.LockedOut;
                return Json(actionResult);
            }
            actionResult.Error = _localizer["InvalidCredentials"];
            actionResult.ErrorCode = (int) ErrorCodes.InvalidCredentials;
            return Json(actionResult);
        }

        [HttpPost("signup"), AllowAnonymous]
        public async Task<JsonResult> Register([FromBody]RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new ActionResult<ModelStateDictionary> {Succeeded = false, WrappedObject = ModelState});
            }
            var user = new User { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            var actionResult = new ActionResult { Succeeded = result.Succeeded, Errors = ModelState.Select(a => (ActionErrors)new ModelActionErrors(a.Key, a.Value.Errors)).ToList() };
            if (!result.Succeeded) return Json(actionResult);
            await _signInManager.SignInAsync(user, isPersistent: false);
            _logger.LogInformation((int)ErrorCodes.AccountCreated, "User created a new account with password.");
            return Json(actionResult);
        }

        [HttpGet("logout"), Authorize]
        public async Task<JsonResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return Json(new ActionResult());
        }

        [HttpGet("loggedin"), AllowAnonymous]
        public JsonResult IsLoggedIn()
        {
            var result = new ActionResult { Succeeded = _signInManager.IsSignedIn(User) };
            return Json(result);
        }
    }
}