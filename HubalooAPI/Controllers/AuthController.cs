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

namespace WebAPIv2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // private readonly ILogger<AuthController> _logger;
        private readonly IAuthManager _authManager;
        private readonly IConfiguration _configuration;
        public AuthController(IAuthManager authManager, ILogger<AuthController> logger, IConfiguration configuration)
        {
            _authManager = authManager;
            _configuration = configuration;
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
                return BadRequest("Username already exists.");
            }

            var userToCreate = new User
            {
                Email = userForRegisterDto.Email
            };

            var createdUser = await _authManager.Signup(userToCreate, userForRegisterDto.Password);

            return Created("", new
            {
                message = "User successfully created.",
                user = new
                {
                    user_id = createdUser.Id,
                    email = createdUser.Email
                }

            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/[controller]/Login")]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            if (String.IsNullOrEmpty(userLoginDto.Email.Trim()) || String.IsNullOrEmpty(userLoginDto.Password.Trim()))
            {
                return Unauthorized("Username and Password are required");
            }

            var emailExists = await _authManager.UserExists(userLoginDto.Email);
            if (!emailExists)
            {
                return Unauthorized("User does not exist in database");
            }

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

        private async Task<bool> UserExists(string username)
        {
            return await _authManager.UserExists(username);

        }
    }
}