namespace MemoryApi.Core.DbModels
{
    public class DbLanguagePair
    {
        public DbLanguagePair() { }

        public DbLanguagePair(int langFromId, int langToId)
        {
            LanguageFromId = langFromId;
            LanguageToId = langToId;
        }

        public int LanguageFromId { get; set; }
        public int LanguageToId { get; set; }
    }
}