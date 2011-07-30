using System;

namespace Euclid.Composites.Mvc.MappingPipeline
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