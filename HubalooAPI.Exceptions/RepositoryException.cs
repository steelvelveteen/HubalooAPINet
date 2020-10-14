using System;

namespace HubalooAPI.Exceptions
{
    public class RepositoryException : Exception
    {
        public RepositoryException()
        { }

        public RepositoryException(string message)
        : base($"Repository exception {message}")
        { }

        public RepositoryException(string message, Exception inner)
        : base(message, inner)
        { }
    }
}