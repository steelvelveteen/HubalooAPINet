using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HubalooAPI.Models.Auth;
using HubalooAPI.Interfaces.BLL;
using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using HubalooAPI.Interfaces.Validators;

namespace HubalooAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // private readonly ILogger<AuthController> _logger;
        private readonly IAuthManager _authManager;
        private readonly IConfiguration _configuration;
        private readonly IAuthValidator _authValidator;
        public AuthController(IAuthManager authManager, IAuthValidator authValidator, IConfiguration configuration)
        {
            _authManager = authManager;
            _configuration = configuration;
            _authValidator = authValidator;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/[controller]/Login")]
        public async Task<ActionResult<UserLoginResponseDto>> Login(UserLoginRequestDto userLoginRequestDto)
        {
            // if (_authValidator.ValidateUserLogin(userLoginRequestDto))
            // {
            //     return Unauthorized("Username and Password are required");
            // }

            if (!await _authManager.UserExists(userLoginRequestDto.Email))
            {
                return StatusCode(401, new
                {
                    message = new[] {
                            "The username or password you have enterd is wrong.NET",
                            "Please try again"
                    }
                });
            }
            try
            {
                var authUser = await _authManager.Login(userLoginRequestDto.Email, userLoginRequestDto.Password);

                return Ok(authUser);
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    message = new[] {
                        "There was a problem loggin in (NET). Try again later."
                    }
                });
            }

        }

        // https://localhost:5001/auth/Signup
        [HttpPost]
        [Route("/[controller]/Signup")]
        public async Task<IActionResult> Signup(UserSignUpRequestDto userSignUpRequestDto)
        {
            // Validate request
            userSignUpRequestDto.Email = userSignUpRequestDto.Email.ToLower();

            // Check if user already exists
            if (await UserExists(userSignUpRequestDto.Email))
            {
                return StatusCode(409, new { message = "Username already exists." });
            }

            var userToCreate = new User
            {
                Email = userSignUpRequestDto.Email
            };

            var createdUser = await _authManager.Signup(userToCreate, userSignUpRequestDto.Password);

            return StatusCode(201, new
            {
                message = "User successfully created.",
                user = new
                {
                    user_id = createdUser.Id,
                    email = createdUser.Email
                }

            });
        }


        private async Task<bool> UserExists(string username)
        {
            return await _authManager.UserExists(username);

        }
    }
}