using System.Threading.Tasks;
using HubalooAPI.Models.Auth;

namespace HubalooAPI.Interfaces.BLL
{
    public interface IAuthManager
    {
        Task<UserLoginResponseDto> Login(string username, string password);

        Task<User> Signup(User user, string password);
        Task<bool> UserExists(string username);
    }
}