using MemoryCore.DbModels;
using MemoryServer.Core.Business.Impl;
using Microsoft.Extensions.Caching.Distributed;

namespace MemoryServer.Core.Business
{
    public class LessonReviewStore : ReviewStore, ILessonReviewStore
    {
        public LessonReviewStore(IDistributedCache cache) : base(cache) {}

        protected override string GetKey(User user) => "Lessons/" + user.Id;
    }
}