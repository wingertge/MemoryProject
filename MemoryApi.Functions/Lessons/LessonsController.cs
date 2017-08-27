using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Autofac;
using JetBrains.Annotations;
using MemoryApi.Core.Business;
using MemoryCore;
using MemoryCore.Models;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace MemoryApi.Functions.Lessons
{
    public static class LessonsController
    {
        [FunctionName("LessonsPut")]
        public static async Task<HttpResponseMessage> Put([NotNull] [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "lessons/put")] HttpRequestMessage msg, [DocumentDB] DocumentClient db, TraceWriter log)
        {
            var req = await ApiRequest.Create<LessonsEditorModel>(msg);
            var model = req.Data;
            var servies = new ServiceLocator(a => a.RegisterInstance(db).As<DocumentClient>().SingleInstance()).Instance;
            var assignmentService = servies.GetInstance<IAssignmentService>();
            var authService = servies.GetInstance<IAuthenticationService>();
            await assignmentService.CreateOrEditAssignment(model.LanguageFromId, model.LanguageToId, model.Reading, model.Pronunciation, model.Meaning, await authService.GetUserId(), model.Id);
            return msg.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(new ActionResult { Succeeded = true }));
        }
    }
}
