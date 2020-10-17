using HubalooAPI.Models.Auth;

namespace HubalooAPI.Interfaces.Validators
{
    public interface IAuthValidator
    {
        // bool ValidateUserLogin(UserLoginRequestDto userLoginRequestDto);
        void ValidateUserLogin(UserLoginRequestDto userLoginRequestDto);
    }
}