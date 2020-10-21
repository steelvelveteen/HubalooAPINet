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
                user = await _authRepository.GetUserLogin(userLoginRequestDto.Email, userLoginRequestDto.Password);

                VerifyPasswordHash(userLoginRequestDto.Password, user.PasswordHash, user.PasswordSalt);

                token = GenerateSecurityToken(user.Email);
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

        public async Task<UserSignUpResponseDto> Signup(UserSignUpRequestDto userSignUpRequestDto)
        {
            if (await UserExists(userSignUpRequestDto.Email))
            {
                throw new UnauthorizedAccessException("User already exists.");
            }
            var createdUser = await _authRepository.Signup(userSignUpRequestDto.Email, userSignUpRequestDto.Password);

            return new UserSignUpResponseDto
            {
                UserId = createdUser.Id,
                Email = createdUser.Email
            };
        }

        public async Task<ResetPasswordResponseDto> ResetPassword(ResetPasswordRequestDto resetPasswordRequestDto)
        {
            if (!await UserExists(resetPasswordRequestDto.Email))
            {
                throw new UnauthorizedAccessException("User does not exist");
            }

            // Get the user from repository - database
            var user = await _authRepository.GetUserLogin(resetPasswordRequestDto.Email, resetPasswordRequestDto.Password);
            User updatedUser;
            try
            {
                updatedUser = await _authRepository.ResetPassword(resetPasswordRequestDto.Email, resetPasswordRequestDto.Password);
                // update database
            }
            catch (Exception ex)
            {
                throw new ApiException("Failed to update user to database.", ex);
            }

            return new ResetPasswordResponseDto
            {
                UserId = updatedUser.Id,
                Email = updatedUser.Email
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

        private JwtSecurityToken GenerateSecurityToken(string email)
        {
            var claims = new[] {
                    new Claim(ClaimTypes.Email, email)
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
            return token;
        }
    }
}