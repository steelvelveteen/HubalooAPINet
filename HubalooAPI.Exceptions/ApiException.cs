using System;

namespace HubalooAPI.Exceptions
{
    public class ApiException : Exception
    {
        public ApiException() { }
        public ApiException(string message) : base($"Api exception ==>> {message}") { }
        public ApiException(string message, Exception inner)
        : base($"Api exception ==>> {message}", inner) { }
    }
}