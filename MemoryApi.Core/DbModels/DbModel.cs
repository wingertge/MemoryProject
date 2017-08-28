using Microsoft.Azure.Documents;

namespace MemoryApi.Core.DbModels
{
    public class DbModel : Document
    {
        public DbModel()
        {
            Entity = GetType().Name;
        }

        public string Entity { get; }
    }
}