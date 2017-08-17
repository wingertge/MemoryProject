using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MemoryCore;
using MemoryCore.DbModels;
using MemoryCore.Models;
using MemoryServer.Core.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ActionResult = Microsoft.AspNetCore.Mvc.ActionResult;

namespace MemoryServer.Controllers
{
    [Authorize, Route("api/[controller]")]
    public class LessonsController : Controller
    {
        private readonly ILessonService _lessonService;
        private readonly ILanguageService _languageService;
        private readonly IAutocompleteService _autocomplete;
        private readonly UserManager<User> _userManager;

        public enum EditorFields
        {
            Reading,
            Pronunciation,
            Meaning
        }

        public LessonsController(ILessonService lessonService, ILanguageService languageService, IAutocompleteService autocomplete, UserManager<User> userManager, IOptions<Config> config)
        {
            _lessonService = lessonService;
            _languageService = languageService;
            _autocomplete = autocomplete;
            _userManager = userManager;
        }

        [HttpPost("put")]
        public async Task<ActionResult> Put([FromBody]LessonsEditorModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _lessonService.CreateOrEditAssignment(model.LanguageFromId, model.LanguageToId, model.Reading, model.Pronunciation, model.Meaning, user, model.Id);
            return Json(new MemoryCore.ActionResult {Succeeded = result != null});
        }

        [HttpGet("language-list/{id}")]
        public async Task<ActionResult> LanguageList(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            return Json(new JsonResult<LanguagesPair>(await _languageService.GetLanguagesByPopularity(user, id)));
        }

        [HttpGet("current-count")]
        public async Task<ActionResult> CurrentCount()
        {
            var user = await _userManager.GetUserAsync(User);
            return Json(new JsonResult<int>(await _lessonService.GetPendingLessonReviewCount(user)));
        }

        [HttpGet("current-list")]
        public async Task<ActionResult> CurrentList()
        {
            var user = await _userManager.GetUserAsync(User);
            var result = new JsonResult<List<LessonAssignment>>(await _lessonService.GetUserAssignments(user));
            return Json(result);
        }

        [HttpGet("editor-autocomplete")]
        public async Task<ActionResult> EditorAutocomplete([FromQuery] string query, [FromQuery] EditorFields field, [FromQuery] int langFromId, [FromQuery] int langToId)
        {
            Expression<Func<Lesson, bool>> genericConditionals = a => a.LanguageFrom.Id == langFromId && a.LanguageTo.Id == langToId;
            List<string> result;
            switch (field)
            {
                case EditorFields.Reading:
                    result = await _autocomplete.GetAutocompleteResults(query, a => a.Lessons, a => a.Reading,
                        genericConditionals, a => a.Reading.Contains(query));
                    break;
                case EditorFields.Pronunciation:
                    result = await _autocomplete.GetAutocompleteResults(query, a => a.Lessons, a => a.Pronunciation,
                        genericConditionals, a => a.Pronunciation.Contains(query));
                    break;
                case EditorFields.Meaning:
                    result = await _autocomplete.GetAutocompleteResults(query, a => a.Lessons, a => a.Meaning,
                        genericConditionals, a => a.Meaning.Contains(query));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(field), field, null);
            }
            return Json(new JsonResult<List<string>>(result));
        }
    }
}