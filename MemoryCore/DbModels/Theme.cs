using System;
using Newtonsoft.Json;

namespace MemoryCore.DbModels
{
    public class Theme
    {
        [JsonIgnore]
        public Guid OwnerId { get; set; }
        [JsonIgnore]
        public User Owner { get; set; }

        public string BackgroundPrimary { get; set; }
        public string BorderPrimary { get; set; }
        public string TextPrimary { get; set; }

        public string BackgroundSecondary { get; set; }
        public string BorderSecondary { get; set; }
        public string TextSecondary { get; set; }

        public string BackgroundTertiary { get; set; }
        public string BorderTertiary { get; set; }
        public string TextTertiary { get; set; }
    }
}