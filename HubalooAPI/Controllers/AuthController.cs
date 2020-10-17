using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HubalooAPI.Models.Auth;
using HubalooAPI.Interfaces.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace HubalooAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManager _authManager;
        private readonly IConfiguration _configuration;
        public AuthController(IAuthManager authManager, IConfiguration configuration)
        {
            _authManager = authManager;
            _configuration = configuration;
        }

        // https://localhost:5001/auth/login
        [AllowAnonymous]
        [HttpPost]
        [Route("/[controller]/Login")]
        public async Task<UserLoginResponseDto> Login(UserLoginRequestDto userLoginRequestDto)
        {
            var authUser = await _authManager.Login(userLoginRequestDto);
            return authUser;
        }

        // https://localhost:5001/auth/signup
        [HttpPost]
        [Route("/[controller]/Signup")]
        public async Task<UserSignUpResponseDto> Signup(UserSignUpRequestDto userSignUpRequestDto)
        {
            // Validate request
            userSignUpRequestDto.Email = userSignUpRequestDto.Email.ToLower();
            var newUserSignUp = new User
            {
                Email = userSignUpRequestDto.Email
            };

            var createdUser = await _authManager.Signup(newUserSignUp, userSignUpRequestDto.Password);
            return createdUser;
        }
    }
}