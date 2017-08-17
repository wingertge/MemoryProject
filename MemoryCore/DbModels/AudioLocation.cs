using System;
using Newtonsoft.Json;

namespace MemoryCore.DbModels
{
    public class AudioLocation
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string RelFileName { get; set; }
        [JsonIgnore]
        public User Uploader { get; set; }
    }
}