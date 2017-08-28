using Newtonsoft.Json;

namespace MemoryApi.Core.DbModels
{
    public class Theme : DbModel
    {
        [JsonIgnore]
        public string UserId { get; set; }

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