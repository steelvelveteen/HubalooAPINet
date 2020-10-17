namespace HubalooAPI.Models.Auth
{
    public class UserSignUpResponseDto
    {
        public string Message { get; set; } = "User successfully created.";
        public int UserId { get; set; }
        public string Email { get; set; }
    }
}