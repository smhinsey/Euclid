using System;

namespace Euclid.Composites.Mvc.Maps
{
    public interface IMapper<in TSource, out TDestination> : IMapper
    {
        Type Source { get; }
        Type Destination { get; }

        TDestination Map(TSource commandMetadata);
    }

    public interface IMapper
    {
        
    }
}