using System;

namespace Euclid.Composites.Mvc.MappingPipeline
{
    public class MapperNotFoundException : Exception
    {
        public MapperNotFoundException(Type sourceType, Type destinationType) : base(string.Format("No mapper is registered that converts from {0} to {1}", sourceType.FullName, destinationType.FullName))
        {
        }
    }
}