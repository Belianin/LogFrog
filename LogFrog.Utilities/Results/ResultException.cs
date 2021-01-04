using System;

namespace LogFrog.Utilities.Results
{
    public class ResultException : Exception
    {
        public ResultException(string errorMessage) : base(errorMessage) {}
    }
}