using System.Collections.Generic;
using Newtonsoft.Json;

namespace MemoryCore.DbModels
{
    public class Tag
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Value { get; set; }
        [JsonIgnore]
        public List<PostTag> Posts { get; set; } = new List<PostTag>();
    }
}