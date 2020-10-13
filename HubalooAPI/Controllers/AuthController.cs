using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HubalooAPI.Models.DTOs;
using HubalooAPI.Models.Auth;
using HubalooAPI.Interfaces.BLL;

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


        // https://localhost:5001/auth/Signup
        [HttpPost]
        [Route("/[controller]/Register")]
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

            var createdUser = await _authManager.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201);
        }

        private async Task<bool> UserExists(string username)
        {
            return await _authManager.UserExists(username);

        }
    }
}