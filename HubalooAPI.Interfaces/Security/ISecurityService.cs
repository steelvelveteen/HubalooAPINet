namespace HubalooAPI.Interfaces.Security
{
    public interface ISecurityService
    {
        string GenerateSecurityToken(string email);
    }
}