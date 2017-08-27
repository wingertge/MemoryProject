using MemoryCore;

namespace MemoryClient.Web.Models
{
    public class MainPageModel
    {
        public User User { get; }
        public string LocalizationStrings { get; set; }

        public MainPageModel(User user) => User = user;
    }
}