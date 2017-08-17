using System;

namespace MemoryCore.DbModels
{
    public class Review
    {
        public Guid Id { get; set; }
        public LessonAssignment Lesson { get; set; }
        public DateTime ReviewDone { get; set; }
        public int WrongAnswers { get; set; }
        public int StartStage { get; set; }
        public int EndStage { get; set; }
    }
}