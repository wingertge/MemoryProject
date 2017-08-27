using System;
using System.Net;
using System.Threading.Tasks;
using MemoryCore.Models;

namespace MemoryCore
{
    public interface IApiAccess : IDisposable
    {
        Task<bool> IsLoggedIn(CookieCollection toList);
        Task<ActionResultWithCookies> Register(RegisterModel model);
        Task<ActionResultWithCookies> Login(LoginModel model);
        Task<User> GetCurrentUser(CookieCollection cookies);
        Task<T> Query<T>(string url, CookieCollection cookies = null, object model = null);
        Task<QueryResult> Query(string url, CookieCollection cookies = null, string model = null);
        Task<QueryResult> Query(string url, CookieCollection cookies, object model);

        IApiAccess Begin();
    }
}