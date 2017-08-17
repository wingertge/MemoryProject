using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace MemoryClient.Web
{
    public class LoggingHandler : DelegatingHandler
    {
        private readonly ILogger _logger;

        public LoggingHandler(ILogger logger, HttpMessageHandler innerHandler) : base(innerHandler)
        {
            _logger = logger;
        }

        [ItemNotNull]
        protected override async Task<HttpResponseMessage> SendAsync([NotNull] HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var result = "Request:\n";
            result += request + "\n";
            if (request.Content != null)
                result += await request.Content.ReadAsStringAsync() + "\n";
            _logger.LogInformation(result);

            var response = await base.SendAsync(request, cancellationToken);

            result = "Response:";
            result += response.ToString();
            if (response.Content != null)
            {
                result += await response.Content.ReadAsStringAsync();
            }
            _logger.LogInformation(result);

            return response;
        }
    }
}