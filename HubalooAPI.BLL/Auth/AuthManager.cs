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

        public Task<User> Login(string email, string password)
        {
            return _authRepository.Login(email, password);
        }

        public Task<User> Signup(User user, string password)
        {
            return _authRepository.Signup(user, password);
        }

        public Task<bool> UserExists(string email)
        {
            return _authRepository.UserExists(email);
        }
    }
}