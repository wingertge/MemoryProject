using System;
using System.Threading.Tasks;

namespace MemoryServer.Core.Database
{
    public interface ITransactionFactory<out TContext>
    {
        ITransaction<TReturn> CreateTransaction<TReturn>(Func<TContext, TReturn> action);
        ITransaction<Task<TReturn>> CreateAsyncTransaction<TReturn>(Func<TContext, Task<TReturn>> action);
    }
}