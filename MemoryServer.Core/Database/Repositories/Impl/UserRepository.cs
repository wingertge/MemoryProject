using System;
using System.Threading.Tasks;
using MemoryCore.DbModels;

namespace MemoryServer.Core.Database.Repositories.Impl
{
    public class UserRepository : IUserRepository
    {
        private readonly ITransactionFactory<MemoryContext> _db;
        public UserRepository(ITransactionFactory<MemoryContext> db) => _db = db;

        public Task<User> CreateOrUpdateUserAsync(User user)
        {
            return _db.CreateAsyncTransaction(async context =>
            {
                var result = user.Id == Guid.Empty ? context.Users.Add(user).Entity : context.Users.Update(user).Entity;
                await context.SaveChangesAsync();
                return result;
            }).Run();
        }
    }
}