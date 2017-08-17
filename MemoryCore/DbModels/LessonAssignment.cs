using System;
using MemoryCore.Models;
using Newtonsoft.Json;

namespace MemoryCore.DbModels
{
    public class LessonAssignment
    {
        public Guid Id { get; set; }
        [JsonIgnore]
        public User Owner { get; set; }
        public Lesson Lesson { get; set; }
        public int Stage { get; set; }
        public DateTime NextReview { get; set; }

        public string ValueFromFieldType(ReviewField field)
        {
            switch (field)
            {
                case ReviewField.Reading: return Lesson.Reading;
                case ReviewField.Pronunciation: return Lesson.Pronunciation;
                case ReviewField.Meaning: return Lesson.Meaning;
                default: throw new ArgumentOutOfRangeException(nameof(field), field, null);
            }
        }
    }
}