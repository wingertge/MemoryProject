using System;

namespace MemoryCore.DbModels
{
    public class BurnedAssignment
    {
        public string OwnerId { get; set; }
        public Guid LessonId { get; set; }

        public Lesson Lesson { get; set; }
    }
}