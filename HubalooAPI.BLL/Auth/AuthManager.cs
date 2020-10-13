using System.Threading.Tasks;
using HubalooAPI.Interfaces.BLL;
using HubalooAPI.Interfaces.Dal;
using HubalooAPI.Models.Auth;

namespace HubalooAPI.BLL
{
    public class AuthManager : IAuthManager
    {
        private readonly IAuthRepository _authRepository;

        public AuthManager(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public Task<User> Login(string username, string password)
        {
            return _authRepository.Login(username, password);
        }

        public Task<User> Register(User user, string password)
        {
            return _authRepository.Register(user, password);
        }

        public Task<bool> UserExists(string username)
        {
            return _authRepository.UserExists(username);
        }
    }
}