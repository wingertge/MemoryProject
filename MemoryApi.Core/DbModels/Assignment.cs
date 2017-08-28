using System;
using System.Collections.Generic;
using MemoryCore.Models;
using Newtonsoft.Json;

namespace MemoryApi.Core.DbModels
{
    public class Assignment : DbModel
    {
        public string UserId { get; set; }
        public Lesson Lesson { get; set; }
        public int Stage { get; set; }
        public long NextReview { get; set; }
        public bool Active { get; set; }
        public bool Burned { get; set; }

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