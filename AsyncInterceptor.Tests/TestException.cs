using System;

namespace AsyncInterceptor.Tests
{
    public class TestException : Exception
    {
        public TestException(string message) : base(message)
        {
        }
    }
}