using System;

namespace MemoryCore.DbModels
{
    public class PostTag
    {
        public int TagId { get; set; }
        public Guid PostId { get; set; }

        public Tag Tag { get; set; }
        public Post Post { get; set; }
    }
}