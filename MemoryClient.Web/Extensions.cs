using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MemoryClient.Web
{
    public static class Extensions
    {
        public static Task<HttpResponseMessage> PostAsync([NotNull] this HttpClient client, string url, object model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            return client.PostAsync(url, content);
        }

        [NotNull]
        public static CookieCollection GetCookies([NotNull] this Controller controller)
        {
            var collection = new CookieCollection();
            foreach (var cookie in controller.Request.Cookies)
            {
                collection.Add(new Cookie(cookie.Key, cookie.Value));
            }
            return collection;
        }

        public static void SetCookies([NotNull] this Controller controller, [NotNull] CookieCollection cookies, [CanBeNull] ILogger logger = null)
        {
            foreach (Cookie cookie in cookies)
                controller.Response.Cookies.Append(cookie.Name, cookie.Value);
        }
    }
}