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


        // https://localhost:5001/auth/register
        [HttpPost]
        [Route("/[controller]/Register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            // Validate request
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            // Check if user already exists
            if (await UserExists(userForRegisterDto.Username))
            {
                return BadRequest("Username already exists.");
            }

            var userToCreate = new User
            {
                Username = userForRegisterDto.Username
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