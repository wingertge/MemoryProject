using System;
using MemoryApi.Core.DbModels;
using MemoryServer;
using MemoryServer.Core;
using Microsoft.Extensions.Options;

namespace MemoryApi.Core.Business.Impl
{
    public class BasicScheduler : IScheduler
    {
        private readonly IOptions<Config> _config;
        public BasicScheduler(IOptions<Config> config) => _config = config;

        public DateTime GetNextReview(Assignment lesson, int grade)
        {
            if (grade == -1) return DateTime.MaxValue;
            return (DateTime.UtcNow + TimeSpan.FromDays(_config.Value.TimeIntervals[grade]))
                .Ceil(TimeSpan.FromMinutes(15));
        }
    }
}