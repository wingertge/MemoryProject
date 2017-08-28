using System.Threading.Tasks;

namespace MemoryApi.Core.Business
{
    public interface IAuthenticationService
    {
        Task<int> SetLanguages(int langFromId, int langToId);
        Task<string> GetUserId();
    }
}