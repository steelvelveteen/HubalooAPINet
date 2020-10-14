using System;

namespace HubalooAPI.Exceptions
{
    public class PreconditionException : Exception
    {
        public PreconditionException()
        {

        }

        public PreconditionException(string something)
        : base($"Some message {something}")
        {

        }
    }
}