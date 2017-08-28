using System;
using System.Collections.Generic;

namespace MemoryApi.Core.DbModels
{
    public class UnorderedUserListEntries : DbModel
    {
        public string ListId { get; set; }
        public List<Lesson> Lessons { get; set; }
    }
}