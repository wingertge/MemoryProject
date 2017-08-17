using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MemoryCore.DbModels
{
    public class UserListEntry
    {
        public Guid OwnerId { get; set; }
        public Guid LessonId { get; set; }

        public int Order { get; set; }
        public virtual Lesson Lesson { get; set; }
        [JsonIgnore]
        public virtual UserList Owner { get; set; }
        public List<ListUpdateTracker> UpdateTrackers { get; set; }
    }
}