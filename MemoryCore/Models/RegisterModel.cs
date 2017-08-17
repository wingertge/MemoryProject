using System.ComponentModel.DataAnnotations;

namespace MemoryCore.Models
{
    public class RegisterModel
    {
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, StringLength(15, MinimumLength = 3), RegularExpression("^[a-zA-Z][a-zA-Z0-9]+$")]
        public string Username { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}