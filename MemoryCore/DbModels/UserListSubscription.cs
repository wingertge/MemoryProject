using System;

namespace MemoryCore.DbModels
{
    public class UserListSubscription
    {
        public Guid ListId { get; set; }
        public Guid UserId { get; set; }
        
        public UserList List { get; set; }
        public User User { get; set; }
    }
}