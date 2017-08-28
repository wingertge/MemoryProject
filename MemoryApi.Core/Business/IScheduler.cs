using System;
using MemoryApi.Core.DbModels;

namespace MemoryApi.Core.Business
{
    public interface IScheduler
    {
        DateTime GetNextReview(Assignment lesson, int grade);
    }
}