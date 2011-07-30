using System;

namespace Euclid.Composites.Mvc.Maps
{
    public class MappingFailedException : Exception
    {
        public MappingFailedException(Type mapperType, Type sourceType, Type destinationType) : base(string.Format("{0}.Map({1}) return null (expected an object of type {2})", mapperType.FullName, sourceType.FullName, destinationType.FullName))
        {
        }
    }
}