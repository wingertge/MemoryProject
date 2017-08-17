using System.Net;
using JetBrains.Annotations;

namespace MemoryCore
{
    public class ActionResultWithCookies : ActionResult
    {
        public ActionResultWithCookies([NotNull] ActionResult actionResult)
        {
            Errors = actionResult.Errors;
            ErrorCode = actionResult.ErrorCode;
            Succeeded = actionResult.Succeeded;
        }

        public ActionResultWithCookies([NotNull] ActionResult result, [NotNull] CookieCollection cookies) : this(result)
        {
            Cookies = cookies;
        }

        public CookieCollection Cookies { get; }
    }
}