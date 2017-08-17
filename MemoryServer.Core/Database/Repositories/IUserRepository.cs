using System.Threading.Tasks;
using MemoryCore.DbModels;

namespace MemoryServer.Core.Database.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateOrUpdateUserAsync(User user);
    }
}