using System;
using System.Collections.Generic;

namespace MemoryApi.Core.DbModels
{
    public class UserListSubscriptions : DbModel
    {
        public string UserId { get; set; }
        public List<string> ListIds { get; set; }
    }
}