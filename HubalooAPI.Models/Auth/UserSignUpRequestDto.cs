using System.ComponentModel.DataAnnotations;

namespace HubalooAPI.Models.Auth
{
    public class UserSignUpRequestDto
    {
        [Required(ErrorMessage = "Email must be provided.",
        AllowEmptyStrings = false)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password must be provided.",
        AllowEmptyStrings = false)]
        public string Password { get; set; }
    }
}