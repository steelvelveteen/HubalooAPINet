using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HubalooAPI.Models.DTOs;
using HubalooAPI.Models.Auth;
using HubalooAPI.Interfaces.BLL;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Text;
using System.Security.Claims;

namespace WebAPIv2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManager _authManager;

        public AuthController(IAuthManager authManager)
        {
            _authManager = authManager;
        }
        // [AllowAnonymous]
        // [HttpPost]
        // [Route("/[controller]/Login")]
        // public IActionResult Login(UserLoginDto userLoginDto)
        // {
        //     if (String.IsNullOrEmpty(userLoginDto.Username.Trim()) || String.IsNullOrEmpty(userLoginDto.Password.Trim()))
        //     {
        //         return Unauthorized("Username and Password are required");
        //     }

        //     var userNameExists = _authManager.UserExists(userLoginDto.Username);
        //     if (!userNameExists)
        //     {
        //         return Unauthorized("User does not exist in database");
        //     }

        //     var authUser = _authManager.Login(userLoginDto);
        //     var claims = new[] {
        //         new Claim(ClaimTypes.Name, authUser.Username)
        //     };

        //     var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

        //     var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        //     var tokenDescriptor = new SecurityTokenDescriptor
        //     {
        //         Subject = new ClaimsIdentity(claims),
        //         Expires = DateTime.Now.AddDays(1),
        //         SigningCredentials = creds
        //     };

        //     var tokenHandler = new JwtSecurityTokenHandler();

        //     var token = tokenHandler.CreateToken(tokenDescriptor);

        //     return Ok(new
        //     {
        //         token = tokenHandler.WriteToken(token)
        //     });

        // }

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

            return StatusCode(201);
        }

        private async Task<bool> UserExists(string username)
        {
            return await _authManager.UserExists(username);

        }
    }
}