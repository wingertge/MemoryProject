using System;
using System.Threading.Tasks;

namespace MemoryServer.Core.Database
{
    public interface ITransaction<out TReturn>
    {
        TReturn Run();
    }
}