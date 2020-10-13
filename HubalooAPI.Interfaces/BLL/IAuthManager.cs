using System.Threading.Tasks;
using HubalooAPI.Models.Auth;

namespace HubalooAPI.Interfaces.BLL
{
    public interface IAuthManager
    {
        Task<User> Register(User user, string password);
        Task<User> Login(string username, string password);
        Task<bool> UserExists(string username);
    }
}