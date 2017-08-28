using System;
using System.Collections.Generic;

namespace MemoryApi.Core.DbModels
{
    public class Post : DbModel
    {
        public string AuthorId { get; set; }
        public List<string> Tags { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
        public long CreatedAt { get; set; }
        public long EditedAt { get; set; }
    }
}