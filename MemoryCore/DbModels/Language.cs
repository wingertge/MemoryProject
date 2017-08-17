namespace MemoryCore.DbModels
{
    public class Language
    {
        public int Id { get; set; }
        public string EnglishName { get; set; }
        public string NativeName { get; set; }
        public string EnglishCountryName { get; set; }
        public string NativeCountryName { get; set; }
        public string IETFTag { get; set; }

        public Language(Languages lang) => Id = (int) lang;
        public Language() { }

        public string GetIETFTag() => IETFTag;

        public enum Languages
        {
            English,
            Japanese,
            Custom
        }
    }
}