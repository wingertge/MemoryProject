using System;

namespace MemoryApi.Core.DbModels
{
    public class UserList : DbModel
    {
        public string CreatorId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Ordered { get; set; }
        public int Likes { get; set; }
        public int Subscriptions { get; set; }
    }
}