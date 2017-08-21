using System.ComponentModel.DataAnnotations;

namespace MemoryCore.Models
{
    public class ChangePasswordModel
    {
        [Required, DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Required, DataType(DataType.Password), Compare("NewPasswordConfirm")]
        public string NewPassword { get; set; }
        [Required, DataType(DataType.Password), Compare("NewPassword")]
        public string NewPasswordConfirm { get; set; }
    }
}