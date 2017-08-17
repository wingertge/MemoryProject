using System;
using MemoryCore.DbModels;
using Microsoft.Extensions.Options;

namespace MemoryServer.Core.Business.Impl
{
    public class BasicScheduler : IScheduler
    {
        private readonly IOptions<Config> _config;
        public BasicScheduler(IOptions<Config> config) => _config = config;

        public DateTime GetNextReview(LessonAssignment lesson, int grade)
        {
            if (grade == -1) return DateTime.MaxValue;
            return (DateTime.UtcNow + TimeSpan.FromDays(_config.Value.TimeIntervals[grade]))
                .Ceil(TimeSpan.FromMinutes(15));
        }
    }
}