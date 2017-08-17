using System;

namespace MemoryCore.DbModels
{
    public class ListUpdateTracker
    {
        public Guid ListId { get; set; }
        public Guid LessonId { get; set; }
        public Guid UserId { get; set; }
        
        public UserListEntry Entry { get; set; }
        public User TrackingUser { get; set; }
    }
}