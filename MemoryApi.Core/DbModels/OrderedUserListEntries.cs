using System;
using System.Collections.Generic;

namespace MemoryApi.Core.DbModels
{
    public class OrderedUserListEntries : DbModel
    {
        public string ListId { get; set; }
        public Dictionary<int, Lesson> Entries { get; set; }
    }
}