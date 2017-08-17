using System.Threading.Tasks;
using MemoryCore.DbModels;

namespace MemoryServer.Core.Database.Repositories
{
    public interface ILessonRepository
    {
        Task<Lesson> FindLessonByContentAsync(int langFrom, int langTo, string reading, string pronunciation, string meaning);
        Task<Lesson> CreateOrUpdateLessonAsync(Lesson lesson);
    }
}