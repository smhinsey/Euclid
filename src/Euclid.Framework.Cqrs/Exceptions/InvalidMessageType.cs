using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Euclid.Framework.Cqrs.Exceptions
{
    public class InvalidMessageTypeException : Exception
    {
        public InvalidMessageTypeException(string message, Exception inner = null) : base(message, inner)
        {
        }
    }
}
