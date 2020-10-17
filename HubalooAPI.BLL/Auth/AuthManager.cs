using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HubalooAPI.Exceptions;
using HubalooAPI.Interfaces.BLL;
using HubalooAPI.Interfaces.Dal;
using HubalooAPI.Interfaces.Validators;
using HubalooAPI.Models.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HubalooAPI.BLL
{
    public class AuthManager : IAuthManager
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;
        private readonly IAuthValidator _authValidator;

        public AuthManager(IAuthRepository authRepository, IAuthValidator authValidator, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
            _authValidator = authValidator;
        }

        public async Task<UserLoginResponseDto> Login(UserLoginRequestDto userLoginRequestDto)
        {
            _authValidator.ValidateCredentials(userLoginRequestDto);

            if (!await UserExists(userLoginRequestDto.Email))
            {
                throw new UnauthorizedAccessException("User does not exist");
            }

            User user = null;
            JwtSecurityToken token = null;
            try
            {
                user = await _authRepository.Login(userLoginRequestDto.Email, userLoginRequestDto.Password);

                VerifyPasswordHash(userLoginRequestDto.Password, user.PasswordHash, user.PasswordSalt);

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
                token = (JwtSecurityToken)tokenHandler.CreateToken(tokenDescriptor);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException(ex.Message);
            }

            return new UserLoginResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
                Token = token.RawData
            };
        }

        public async Task<UserSignUpResponseDto> Signup(User user, string password)
        {
            if (!await UserExists(userLoginRequestDto.Email))
            {
                throw new UnauthorizedAccessException("User does not exist");
            }

            User createdUser = null;
            try
            {
                createdUser = await _authRepository.Signup(user, password);
            }
            catch (Exception ex)
            {
                throw new Exception("Signup failed", ex);
            }

            return new UserSignUpResponseDto
            {
                UserId = createdUser.Id,
                Email = createdUser.Email
            };
        }

        private Task<bool> UserExists(string email)
        {
            return _authRepository.UserExists(email);
        }

        private void VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        throw new UnauthorizedAccessException("The username or password you have entered is wrong. Please try again");
                    }
                }
            }
        }
    }
}