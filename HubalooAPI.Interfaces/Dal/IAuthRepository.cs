using System.Threading.Tasks;
using HubalooAPI.Models.Auth;

namespace HubalooAPI.Interfaces.Dal
{
    public interface IAuthRepository
    {
        Task<User> Signup(string email, string password);
        Task<User> Login(string username, string password);
        Task<bool> UserExists(string username);
    }
}