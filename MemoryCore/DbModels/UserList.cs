using System;
using System.Collections.Generic;

namespace MemoryCore.DbModels
{
    public class UserList
    {
        public Guid Id { get; set; }
        public Language LanguageFrom { get; set; }
        public Language LanguageTo { get; set; }
        public List<UserListEntry> Entries { get; set; } = new List<UserListEntry>();
        public bool Ordered { get; set; } = true;
        public User Owner { get; set; }
        public int Likes { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }
        public List<UserListSubscription> Subscriptions { get; set; } = new List<UserListSubscription>();
    }
}