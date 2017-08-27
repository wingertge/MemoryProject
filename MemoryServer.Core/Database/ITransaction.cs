namespace MemoryServer.Core.Database
{
    public interface ITransaction<out TReturn>
    {
        TReturn Run();
    }
}