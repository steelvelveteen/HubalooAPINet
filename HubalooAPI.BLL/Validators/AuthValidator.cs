using System;
using HubalooAPI.Interfaces.Validators;
using HubalooAPI.Models.Auth;

namespace HubalooAPI.BLL.Validators
{
    public class AuthValidator : IAuthValidator
    {
        public bool ValidateUserLogin(UserLoginRequestDto userLoginRequestDto)
        {
            return String.IsNullOrEmpty(userLoginRequestDto.Email.Trim()) || String.IsNullOrEmpty(userLoginRequestDto.Password.Trim());
        }
    }
}