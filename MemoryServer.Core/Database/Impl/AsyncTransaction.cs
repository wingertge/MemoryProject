using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MemoryServer.Core.Database.Impl
{
    public class AsyncTransaction<TContext, TReturn> : ITransaction<Task<TReturn>> where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly Func<TContext, Task<TReturn>> _action;
        private readonly ILogger<TContext> _logger;

        public AsyncTransaction(TContext context, Func<TContext, Task<TReturn>> action, ILogger<TContext> logger)
        {
            _context = context;
            _action = action;
            _logger = logger;
        }

        public async Task<TReturn> Run()
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var result = await _action(_context);
                    transaction.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    _logger.LogError(e, "Exception in database operation:");
                    return default(TReturn);
                }
            }
        }
    }
}