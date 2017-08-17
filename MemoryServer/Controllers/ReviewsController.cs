using System.Threading.Tasks;
using MemoryCore;
using MemoryCore.DbModels;
using MemoryCore.Models;
using MemoryServer.Core;
using MemoryServer.Core.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MemoryServer.Controllers
{
    [Authorize, Route("api/[controller]")]
    public class ReviewsController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly UserManager<User> _userManager;

        public ReviewsController(IReviewService reviewService, UserManager<User> userManager)
        {
            _reviewService = reviewService;
            _userManager = userManager;
        }

        [HttpGet("current-count")]
        public async Task<JsonResult> CurrentCount()
        {
            var user = await _userManager.GetUserAsync(User);
            return Json(new JsonResult<int>(await _reviewService.GetPendingReviewCountAsync(user)));
        }

        [HttpGet("next-review-time")]
        public async Task<JsonResult> NextReviewTime()
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _reviewService.GetNextReviewTimeAsync(user);
            return Json(new JsonResult<long>(result.ToUnixMillis()));
        }

        [HttpGet("next-hour-count")]
        public async Task<JsonResult> NextHourCount()
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _reviewService.GetNextHourUpcomingReviewCountAsync(user);
            return Json(new JsonResult<int>(result));
        }

        [HttpGet("next-day-count")]
        public async Task<JsonResult> NextDayCount()
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _reviewService.GetNextDayUpcomingReviewCountAsync(user);
            return Json(new JsonResult<int>(result));
        }

        [HttpGet("next-review")]
        public async Task<JsonResult> Next()
        {
            var user = await _userManager.GetUserAsync(User);
            return Json(new JsonResult<ReviewModel>(await _reviewService.GetNextReviewAsync(user)));
        }

        [HttpPost("submit-review")]
        public async Task<JsonResult> Submit([FromBody] ReviewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            return Json(new MemoryCore.ActionResult { Succeeded = await _reviewService.SubmitReviewAsync(user, model.AssignmentId, model.FieldFrom, model.FieldTo, model.To) });
        }
    }
}