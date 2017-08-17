using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MemoryServer.Core.Database.Impl
{
    public class Transaction<TContext, TReturn> : ITransaction<TReturn> where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly Func<TContext, TReturn> _action;
        private readonly ILogger<TContext> _logger;

        public Transaction(TContext context, Func<TContext, TReturn> action, ILogger<TContext> logger)
        {
            _context = context;
            _action = action;
            _logger = logger;
        }

        public TReturn Run()
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var result = _action(_context);
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