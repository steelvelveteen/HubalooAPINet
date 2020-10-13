using Dapper;
using HubalooAPI.Dal.Database;
using HubalooAPI.Interfaces.Dal;
using HubalooAPI.Models.Auth;
using System.Threading.Tasks;

namespace HubalooAPI.Dal.Auth
{
    public class AuthRepository : IAuthRepository
    {

        private readonly IDatabase _database;

        public AuthRepository(IDatabase database)
        {
            _database = database;
        }

        public async Task<User> Login(string username, string password)
        {
            var sql = $"Select * from Users WHERE username = username ";
            var user = await _database.QueryFirstOrDefaultAsync<User>(sql);

            if (user == null)
            {
                return null;
            }

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return user;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordSalt;
            CreatePasswordHash(password, out byte[] passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            var parameters = new DynamicParameters();
            parameters.Add("@Username", user.Username);
            parameters.Add("@PasswordHash", user.PasswordHash);
            parameters.Add("@PasswordSalt", user.PasswordSalt);

            var sql = $"insert into [dbo].[Users]([Username], [PasswordHash], [PasswordSalt]) values (@Username, @PasswordHash, @PasswordSalt)";

            await _database.ExecuteAsync(sql, parameters);

            return user;
        }

        public async Task<bool> UserExists(string username)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Username", username);
            // var sql = "Select [Username] from [dbo].[Users] WHERE [Username] = @Username ";
            var sql = "Select username from users WHERE username = @Username ";

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