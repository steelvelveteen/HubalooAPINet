using System.ComponentModel.DataAnnotations;

namespace HubalooAPI.Models.Attributes
{
    public class PasswordAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value.ToString().Length > 8;
        }
    }
}