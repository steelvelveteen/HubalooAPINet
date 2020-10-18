using Dapper;
using HubalooAPI.Dal.Database;
using HubalooAPI.Exceptions;
using HubalooAPI.Interfaces.Dal;
using HubalooAPI.Models.Auth;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace HubalooAPI.Dal.Auth
{
    public class AuthRepository : IAuthRepository
    {

        private readonly IDatabase _database;
        private readonly ILogger _logger;

        public AuthRepository(IDatabase database, ILogger<AuthRepository> logger)
        {
            _database = database;
            _logger = logger;
        }

        public async Task<User> Login(string useremail, string password)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Email", useremail);
            var sql = "Select * from Use WHERE email = @Email";
            // Following deliberately set to fail Internal Server Error
            // var sql = "Select * from use WHERE email = @Email ";
            User user;
            try
            {
                user = await _database.QueryFirstOrDefaultAsync<User>(sql, parameters);
                return user;
            }
            catch (Exception e)
            {
                _logger.LogError($"MSG:{e.Message} || SRC:{e.Source} || S.TRACE{e.StackTrace}");
                throw new RepositoryException("Login failed at repository level.");
            }
        }

        public async Task<User> Signup(string email, string password)
        {
            byte[] passwordSalt;
            CreatePasswordHash(password, out byte[] passwordHash, out passwordSalt);

            User newUser = new User
            {
                Email = email,
            };

            newUser.PasswordHash = passwordHash;
            newUser.PasswordSalt = passwordSalt;

            var parameters = new DynamicParameters();
            parameters.Add("@Email", newUser.Email);
            parameters.Add("@PasswordHash", newUser.PasswordHash);
            parameters.Add("@PasswordSalt", newUser.PasswordSalt);

            var sql = $"insert into Use (email, PasswordHash, PasswordSalt) values (@Email, @PasswordHash, @PasswordSalt)";

            try
            {
                await _database.ExecuteAsync(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Failed to insert user to database.", ex);
            }
            // Return the user inserted into DB
            parameters.Add("@Email", newUser.Email);
            var newSql = "Select * from users WHERE email = @Email";
            var createdUser = await _database.QueryFirstOrDefaultAsync<User>(newSql, parameters);

            return createdUser;
        }

        public async Task<bool> UserExists(string email)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Email", email);
            var sql = "Select email from users WHERE email = @Email ";

            var user = await _database.QueryFirstOrDefaultAsync<User>(sql, parameters);

            if (user == null)
            {
                return false;
            }
            return true;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }
    }
}