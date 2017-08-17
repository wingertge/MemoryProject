using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MemoryCore.DbModels
{
    public class Lesson
    {
        public Guid Id { get; set; }
        public Language LanguageFrom { get; set; }
        public Language LanguageTo { get; set; }
        public List<AudioLocation> Audio { get; set; } = new List<AudioLocation>();
        public string Reading { get; set; }
        public string Pronunciation { get; set; }
        public string Meaning { get; set; }

        [JsonIgnore]
        public List<UserListEntry> ListEntries { get; set; } = new List<UserListEntry>();
    }
}