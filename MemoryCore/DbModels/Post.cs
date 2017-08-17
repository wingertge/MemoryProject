using System;
using System.Collections.Generic;

namespace MemoryCore.DbModels
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime EditedAt { get; set; }
        public List<PostTag> Tags { get; set; } = new List<PostTag>();
        public User Author { get; set; }
    }
}