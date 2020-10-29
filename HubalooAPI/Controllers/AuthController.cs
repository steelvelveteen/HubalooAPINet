using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HubalooAPI.Models.Auth;
using HubalooAPI.Interfaces.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HubalooAPI.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManager _authManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthManager authManager, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _authManager = authManager;
            _configuration = configuration;
            _logger = logger;
        }

        // https://localhost:5000/auth/login
        [HttpPost]
        [Route("/[controller]/Login")]
        public async Task<UserLoginResponseDto> Login(UserLoginRequestDto userLoginRequestDto)
        {
            _logger.LogInformation("Login request performed");
            var authUser = await _authManager.Login(userLoginRequestDto);
            return authUser;
        }

        // https://localhost:5000/auth/signup
        [HttpPost]
        [Route("/[controller]/Signup")]
        public async Task<UserSignUpResponseDto> Signup(UserSignUpRequestDto userSignUpRequestDto)
        {
            userSignUpRequestDto.Email = userSignUpRequestDto.Email.ToLower();

            var createdUser = await _authManager.Signup(userSignUpRequestDto);
            return createdUser;
        }

        // https://localhost:5000/auth/resetpassword
        [HttpPost]
        [Route("/[controller]/ResetPassword")]
        public async Task<ResetPasswordResponseDto> ResetPassword(ResetPasswordRequestDto resetPasswordRequestDto)
        {
            var updatedUser = await _authManager.ResetPassword(resetPasswordRequestDto);
            return updatedUser;
        }
    }
}