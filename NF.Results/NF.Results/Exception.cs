using System;

namespace NF.Results
{
    // ref: https://docs.microsoft.com/en-us/dotnet/standard/exceptions/best-practices-for-exceptions
    // ref: https://blog.gurock.com/articles/creating-custom-exceptions-in-dotnet/

    [Serializable]
    public class UnExpectedException : Exception
    {
        public UnExpectedException(string message) : base(message)
        {
        }
    }

    [Serializable]
    public class ArgumentNullException : Exception
    {
        public ArgumentNullException(string message) : base(message)
        {
        }
    }

    [Serializable]
    public class UnwrapException : Exception
    {
        public UnwrapException(string message) : base(message)
        {
        }
    }
}