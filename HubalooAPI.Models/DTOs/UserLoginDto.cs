using System.ComponentModel.DataAnnotations;

namespace HubalooAPI.Models.DTOs
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Email must be provided.",
        AllowEmptyStrings = false)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password must be provided.",
        AllowEmptyStrings = false)]
        public string Password { get; set; }
    }
}