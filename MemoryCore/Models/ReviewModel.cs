using System;

namespace MemoryCore.Models
{
    public class ReviewModel
    {
        public Guid AssignmentId { get; set; }
        public ReviewField FieldFrom { get; set; }
        public ReviewField FieldTo { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Solution { get; set; }
    }

    public enum ReviewField
    {
        Reading,
        Meaning,
        Pronunciation
    }
}