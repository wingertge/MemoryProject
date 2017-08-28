using Microsoft.Extensions.Caching.Distributed;

namespace MemoryApi.Core.Business.Impl
{
    public class LessonReviewStore : ReviewStore, ILessonReviewStore
    {
        public LessonReviewStore(IDistributedCache cache) : base(cache) {}

        protected override string GetKey(string userId) => "Lessons/" + userId;
    }
}