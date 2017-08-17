using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MemoryCore.Models
{
    public class LoginModel
    {
        [Required, DisplayName("Username/Email")]
        public string Identifier { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [DisplayName("Remember Me")]
        public bool Remember { get; set; }
    }
}