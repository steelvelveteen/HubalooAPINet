using System;

namespace HubalooAPI.Exceptions
{
    public class PreconditionException : Exception
    {

        public PreconditionException()
        { }

        public PreconditionException(string message)
        : base($"Precondition exception thrown: {message}")
        { }

        public PreconditionException(string message, Exception inner)
               : base(message, inner)
        { }

    }
}