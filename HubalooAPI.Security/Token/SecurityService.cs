using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HubalooAPI.Interfaces.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HubalooAPI.Security.Token
{
    public class SecurityService : ISecurityService
    {
        private readonly IConfiguration _configuration;

        public SecurityService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // public JwtSecurityToken GenerateSecurityToken(string email)

        public string GenerateSecurityToken(string email)
        {
            var claims = new[] {
                    new Claim(ClaimTypes.Email, email),
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:APIKEY").Value));
            var issuer = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Issuer").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // var tokenDescriptor = new SecurityTokenDescriptor
            // {
            //     Subject = new ClaimsIdentity(claims),
            //     Expires = DateTime.Now.AddDays(1),
            //     SigningCredentials = creds,
            // };

            // var tokenHandler = new JwtSecurityTokenHandler();
            // var token = (JwtSecurityToken)tokenHandler.CreateToken(tokenDescriptor);
            // return token;
            var token = new JwtSecurityToken(
            _configuration["AppSettings:Issuer"], // Issuer
            _configuration["AppSettings:Issuer"], // Audience
            claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public void VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
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