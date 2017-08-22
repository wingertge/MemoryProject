using System.Threading.Tasks;
using MemoryClient.Web.Models;
using MemoryCore;
using MemoryCore.Models;
using Microsoft.AspNetCore.Mvc;
using ActionResult = Microsoft.AspNetCore.Mvc.ActionResult;

namespace MemoryClient.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IApiAccess _apiAccess;

        public HomeController(IApiAccess apiAccess)
        {
            _apiAccess = apiAccess;
        }

        [HttpGet("signup")]
        public async Task<ActionResult> Register()
        {
            using (_apiAccess.Begin())
            {
                if (await _apiAccess.IsLoggedIn(this.GetCookies())) return Redirect("home");
                return View(new RegisterModel());
            }
        }

        [HttpPost("signup")]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            using (_apiAccess.Begin())
            {
                var result = await _apiAccess.Register(model);
                this.SetCookies(result.Cookies);
                if (result.Succeeded) return Redirect("home");
                foreach (var item in result.Errors)
                foreach (var error in item.Value)
                    ModelState.AddModelError(item.Key, error);

                return View(model);
            }
        }

        [HttpGet("login")]
        public async Task<ActionResult> Login()
        {
            using (_apiAccess.Begin())
            {
                if (await _apiAccess.IsLoggedIn(this.GetCookies())) return Redirect("home");
                return View(new LoginModel());
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginModel model)
        {
            using (_apiAccess.Begin())
            {
                var result = await _apiAccess.Login(model);
                this.SetCookies(result.Cookies);
                if (result.Succeeded) return Redirect("home");
                var error = (ErrorCodes) result.ErrorCode;
                ModelState.AddModelError(error.ToString(), result.Error);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            using (_apiAccess.Begin()) if (!await _apiAccess.IsLoggedIn(this.GetCookies())) return Redirect("login");
            using (_apiAccess.Begin())
            {
                var user = await _apiAccess.GetCurrentUser(this.GetCookies());
                return View(new MainPageModel(user));
            }
        }

        [HttpGet("api/{apiController}/{apiAction}/{*args}")]
        public async Task<ActionResult> ApiCall(string apiController, string apiAction, string args)
        {
            using (_apiAccess.Begin())
            {
                var result = await _apiAccess.Query(apiController + "/" + apiAction + "/" + args, this.GetCookies());
                if(result.Cookies != null) this.SetCookies(result.Cookies);
                return Content(result.Json, "application/json");
            }
        }

        [HttpPost("api/{apiController}/{apiAction}/{*args}")]
        public async Task<ActionResult> ApiCall([FromBody]string model, string apiController, string apiAction, string args)
        {
            using (_apiAccess.Begin())
            {
                var result = await _apiAccess.Query(apiController + "/" + apiAction + "/" + args, this.GetCookies(), model);
                if (result.Cookies != null) this.SetCookies(result.Cookies);
                return Content(result.Json, "application/json");
            }
        }
    }
}