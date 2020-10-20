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

        // https://localhost:5000/auth/login
        [AllowAnonymous]
        [HttpPost]
        [Route("/[controller]/Login")]
        public async Task<UserLoginResponseDto> Login(UserLoginRequestDto userLoginRequestDto)
        {
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