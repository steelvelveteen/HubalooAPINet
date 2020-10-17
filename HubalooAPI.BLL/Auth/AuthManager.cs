using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HubalooAPI.Interfaces.BLL;
using HubalooAPI.Interfaces.Dal;
using HubalooAPI.Models.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HubalooAPI.BLL
{
    public class AuthManager : IAuthManager
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;


        public AuthManager(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        public async Task<UserLoginResponseDto> Login(string email, string password)
        {
            User user = await _authRepository.Login(email, password);

            var claims = new[] {
                new Claim(ClaimTypes.Email, user.Email)
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
            var token = (JwtSecurityToken)tokenHandler.CreateToken(tokenDescriptor);

            return new UserLoginResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
                Token = token.RawData
            };
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