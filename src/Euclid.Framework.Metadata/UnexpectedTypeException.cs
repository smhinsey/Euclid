using System;

namespace Euclid.Framework.Metadata
{
    public class UnexpectedTypeException : Exception
    {
        private Type _expected;
        private readonly Type _received;

        public UnexpectedTypeException(Type expected, Type received)
        {
            _expected = expected;
            _received = received;
        }
    }
}