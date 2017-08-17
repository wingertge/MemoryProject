using System;

namespace MemoryCore.DbModels
{
    public class BurnedAssignment
    {
        public Guid OwnerId { get; set; }
        public Guid LessonId { get; set; }

        public User Owner { get; set; }
        public Lesson Lesson { get; set; }
    }
}