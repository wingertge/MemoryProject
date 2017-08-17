using System;
using MemoryCore.DbModels;

namespace MemoryServer.Core.Business
{
    public interface IScheduler
    {
        DateTime GetNextReview(LessonAssignment lesson, int grade);
    }
}