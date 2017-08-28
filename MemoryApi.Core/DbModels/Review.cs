using System;

namespace MemoryApi.Core.DbModels
{
    public class Review : DbModel
    {
        public string UserId { get; set; }
        public Lesson Lesson { get; set; }
        public long ReviewDone { get; set; }
        public int WrongAnswers { get; set; }
        public int StartStage { get; set; }
        public int EndStage { get; set; }
    }
}