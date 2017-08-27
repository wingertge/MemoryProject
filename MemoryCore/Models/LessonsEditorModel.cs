using System;

namespace MemoryCore.Models
{
    public class LessonsEditorModel
    {
        public Guid Id { get; set; } = Guid.Empty;
        public int LanguageFromId { get; set; }
        public int LanguageToId { get; set; }
        public string Reading { get; set; }
        public string Pronunciation { get; set; }
        public string Meaning { get; set; }
    }
}