using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace MemoryApi.Functions.Auth
{
    public class AzureAdAuthenticationService : IAuthenticationHandler
    {
        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            throw new System.NotImplementedException();
        }

        public Task<AuthenticateResult> AuthenticateAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task ChallengeAsync(AuthenticationProperties properties)
        {
            throw new System.NotImplementedException();
        }

        public Task ForbidAsync(AuthenticationProperties properties)
        {
            throw new System.NotImplementedException();
        }
    }
}