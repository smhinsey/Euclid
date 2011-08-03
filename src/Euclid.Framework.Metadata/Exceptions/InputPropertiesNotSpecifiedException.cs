using System;

namespace Euclid.Framework.Metadata.Exceptions
{
    internal class InputPropertiesNotSpecifiedException : Exception
    {
        public InputPropertiesNotSpecifiedException(Type implementation)  : base(string.Format("the command {0} does not have any input properties declared", implementation.FullName))
        {
        }
    }
}