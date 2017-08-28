using System;
using System.Collections.Generic;

namespace MemoryApi.Core.DbModels
{
    public class Lesson : DbModel
    {
        public DbLanguagePair Languages { get; set; }
        public List<AudioLocation> Audio { get; set; } = new List<AudioLocation>();
        public string Reading { get; set; }
        public string Pronunciation { get; set; }
        public string Meaning { get; set; }
    }
}