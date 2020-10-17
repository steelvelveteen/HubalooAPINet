using System.Threading.Tasks;
using HubalooAPI.Models.Auth;

namespace HubalooAPI.Interfaces.BLL
{
    public interface IAuthManager
    {
        Task<UserLoginResponseDto> Login(UserLoginRequestDto userLoginRequestDto);
        Task<User> Signup(User user, string password);
    }
}