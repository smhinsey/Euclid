using System;

namespace Euclid.Agent.Commands
{
    public class InvalidPropertySetterSpecifiedException : Exception
    {
        public InvalidPropertySetterSpecifiedException(Type propertyValueSetterType)
        {

        }
    }
}