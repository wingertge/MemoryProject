using System;

namespace MemoryApi.Core.DbModels
{
    public class ListUpdateTracker
    {
        public string ListId { get; set; }
        public Lesson Lesson { get; set; }
    }
}