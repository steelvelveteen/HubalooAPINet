using System;
using HubalooAPI.Exceptions;
using HubalooAPI.Interfaces.Validators;
using HubalooAPI.Models.Auth;

namespace HubalooAPI.BLL.Validators
{
    public class AuthValidator : IAuthValidator
    {
        public void ValidateCredentials(UserLoginRequestDto userLoginRequestDto)
        {
            if (String.IsNullOrEmpty(userLoginRequestDto.Email.Trim()) || String.IsNullOrEmpty(userLoginRequestDto.Password.Trim()))
            {
                throw new PreconditionException("Email and Password are required.");
            }
        }
    }
}