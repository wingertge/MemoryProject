using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MemoryCore;
using MemoryCore.DbModels;
using MemoryCore.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MemoryClient.Web
{
    public class ApiAccess : IApiAccess
    {
        private readonly ILogger<ApiAccess> _logger;
        private static Uri BaseUri { get; set; }
        private static Uri HostUri { get; set; }

        private HttpClientHandler _httpHandler;
        private HttpClient _httpClient;

        public ApiAccess([NotNull] IConfigurationRoot config, [NotNull] ILogger<ApiAccess> logger)
        {
            _logger = logger;
            BaseUri = new Uri(config["ApiProtocol"] + "://" + config["ApiHost"] + ":" + config["ApiPort"] + "/api/");
            HostUri = GetHostUri();
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
            _httpHandler?.Dispose();
        }

        public async Task<bool> IsLoggedIn([CanBeNull] CookieCollection cookies)
        {
            _httpHandler.CookieContainer = cookies != null ? GetCookieContainer(cookies) : new CookieContainer();
            var result = await _httpClient.GetStringAsync("auth/loggedin");
            return JsonConvert.DeserializeObject<ActionResult>(result).Succeeded;
        }

        [ItemNotNull]
        public async Task<ActionResultWithCookies> Register([NotNull] RegisterModel model)
        {
            var cookies = new CookieContainer();
            _httpHandler.CookieContainer = cookies;
            var result = await _httpClient.PostAsync("auth/signup", model);
            if (!result.IsSuccessStatusCode)
                _logger.LogWarning(@"Unsuccessful registration: " + (int)result.StatusCode + ": " + result.ReasonPhrase);
            
            return !result.IsSuccessStatusCode ? new ActionResultWithCookies(new ActionResult { Succeeded = false }, cookies.GetCookies(HostUri)) : 
                new ActionResultWithCookies(JsonConvert.DeserializeObject<ActionResult>(await result.Content.ReadAsStringAsync()), cookies.GetCookies(HostUri));
        }

        [ItemNotNull]
        public async Task<ActionResultWithCookies> Login([NotNull] LoginModel model)
        {
            var cookies = new CookieContainer();
            _httpHandler.CookieContainer = cookies;
            var result = await _httpClient.PostAsync("auth/login", model);
            if (!result.IsSuccessStatusCode)
                _logger.LogWarning(@"Unsuccessful login: " + (int)result.StatusCode + ": " + result.ReasonPhrase);
            
            cookies = _httpHandler.CookieContainer;
            return new ActionResultWithCookies(JsonConvert.DeserializeObject<ActionResult>(await result.Content.ReadAsStringAsync()), cookies.GetCookies(HostUri));
        }

        public async Task<User> GetCurrentUser([NotNull] CookieCollection cookies)
        {
            _httpHandler.CookieContainer = GetCookieContainer(cookies);
            var result = await _httpClient.GetAsync("users/current");
            if (!result.IsSuccessStatusCode)
                _logger.LogError(@"Unsuccessful user retrieval: " + (int)result.StatusCode + ": " + result.ReasonPhrase);

            return JsonConvert.DeserializeObject<User>(await result.Content.ReadAsStringAsync());
        }

        public async Task<T> Query<T>([NotNull] string url, [CanBeNull] CookieCollection cookies, [CanBeNull] object model = null)
        {
            _httpHandler.CookieContainer = cookies != null ? GetCookieContainer(cookies) : new CookieContainer();
            string result;
            if (model != null)
            {
                result = await _httpClient.PostAsync(url, model).Result.Content.ReadAsStringAsync();
            }
            else result = await _httpClient.GetStringAsync(url);

            return (T)JsonConvert.DeserializeObject(result, typeof(T));
        }

        [ItemNotNull]
        public async Task<QueryResult> Query([NotNull] string url, [CanBeNull] CookieCollection cookies = null, [CanBeNull] string model = null)
        {
            _httpHandler.CookieContainer = cookies != null ? GetCookieContainer(cookies) : new CookieContainer();
            string result;
            if (model != null)
            {
                result = await _httpClient.PostAsync(url, new StringContent(model, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();
            }
            else result = await _httpClient.GetStringAsync(url);

            return new QueryResult { Cookies = _httpHandler.CookieContainer.GetCookies(HostUri), Json = result };
        }

        [ItemNotNull]
        public async Task<QueryResult> Query([NotNull] string url, [CanBeNull] CookieCollection cookies = null, [CanBeNull] object model = null)
        {
            if (model == null) return await Query(url, cookies, null);
            return await Query(url, cookies, JsonConvert.SerializeObject(model));
        }

        [NotNull]
        public IApiAccess Begin()
        {
            _httpHandler = new HttpClientHandler();
            _httpClient = new HttpClient(new LoggingHandler(_logger, _httpHandler)) {BaseAddress = BaseUri};
            return this;
        }

        [NotNull]
        private static CookieContainer GetCookieContainer([NotNull] CookieCollection cookies)
        {
            var container = new CookieContainer();
            foreach (Cookie cookie in cookies)
            {
                container.Add(HostUri, cookie);
            }
            return container;
        }

        [NotNull]
        private static Uri GetHostUri()
        {
            var oldUri = BaseUri;
            var newUri = new Uri(oldUri.Scheme + "://" + oldUri.Host + ":" + oldUri.Port);
            return newUri;
        }
    }
}