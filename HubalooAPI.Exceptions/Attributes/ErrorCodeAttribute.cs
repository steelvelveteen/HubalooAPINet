using System;

namespace HubalooAPI.Exceptions.Attributes
{
    public sealed class ErrorCodeAttribute : Attribute
    {
        public string Code { get; set; }
        public string Message { get; set; }

        public ErrorCodeAttribute() { }
        public ErrorCodeAttribute(string code, string message)
        {
            Message = message;
            Code = code;
        }
        public static ErrorCodeAttribute GetAttribute(Type T)
        {
            ErrorCodeAttribute attribute = (ErrorCodeAttribute)Attribute.GetCustomAttribute(T, typeof(ErrorCodeAttribute));
            return attribute;
        }
    }
}