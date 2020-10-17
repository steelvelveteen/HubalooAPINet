using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HubalooAPI.Models.DTOs;
using HubalooAPI.Models.Auth;
using HubalooAPI.Interfaces.BLL;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Text;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace HubalooAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // private readonly ILogger<AuthController> _logger;
        private readonly IAuthManager _authManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthManager authManager, ILogger<AuthController> logger, IConfiguration configuration)
        {
            _authManager = authManager;
            _configuration = configuration;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/[controller]/Login")]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            _logger.LogInformation(1101, "Login controller logger activated. {Time}", DateTime.Now);

            if (String.IsNullOrEmpty(userLoginDto.Email.Trim()) || String.IsNullOrEmpty(userLoginDto.Password.Trim()))
            {
                return Unauthorized("Username and Password are required");

            }

            var emailExists = await _authManager.UserExists(userLoginDto.Email);
            if (!emailExists)
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

                var authUser = await _authManager.Login(userLoginDto.Email, userLoginDto.Password);
                var claims = new[] {
                new Claim(ClaimTypes.Email, authUser.Email)
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = creds
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return Ok(new
                {
                    message = "Login Success",
                    user_id = authUser.Id,
                    email = authUser.Email,
                    token = tokenHandler.WriteToken(token)
                });
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
        public async Task<IActionResult> Signup(UserForRegisterDto userForRegisterDto)
        {
            // Validate request
            userForRegisterDto.Email = userForRegisterDto.Email.ToLower();

            // Check if user already exists
            if (await UserExists(userForRegisterDto.Email))
            {
                return StatusCode(409, new { message = "Username already exists." });
            }

            var userToCreate = new User
            {
                Email = userForRegisterDto.Email
            };

            var createdUser = await _authManager.Signup(userToCreate, userForRegisterDto.Password);

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