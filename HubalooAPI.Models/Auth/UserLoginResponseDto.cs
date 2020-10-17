namespace HubalooAPI.Models.Auth
{
    public class UserLoginResponseDto
    {
        public string Message { get; set; } = "Login successfull";
        public int UserId { get; set; }
        public string Email { get; set; }
        public object Token { get; set; }
    }
}