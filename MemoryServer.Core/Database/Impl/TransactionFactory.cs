using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MemoryServer.Core.Database.Impl
{
    public class TransactionFactory<TContext> : ITransactionFactory<TContext> where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly ILogger<TContext> _logger;

        public TransactionFactory(TContext context, ILogger<TContext> logger)
        {
            _context = context;
            _logger = logger;
        }

        public ITransaction<TReturn> CreateTransaction<TReturn>(Func<TContext, TReturn> action)
        {
            return new Transaction<TContext, TReturn>(_context, action, _logger);
        }

        public ITransaction<Task<TReturn>> CreateAsyncTransaction<TReturn>(Func<TContext, Task<TReturn>> action)
        {
            return new AsyncTransaction<TContext,TReturn>(_context, action, _logger);
        }
    }
}