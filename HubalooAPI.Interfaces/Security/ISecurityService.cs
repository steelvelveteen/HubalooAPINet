namespace HubalooAPI.Interfaces.Security
{
    public interface ISecurityService
    {
        string GenerateSecurityToken(string email);
        void VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}