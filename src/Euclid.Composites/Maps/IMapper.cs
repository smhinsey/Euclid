using System;
using System.Collections.Specialized;

namespace Euclid.Composites.Maps
{
    public interface IMapper<in TSource, out TDestination> : IMapper
    {
        TDestination Map(TSource source, NameValueCollection values = null);
    }

    public interface IMapper
    {
        Type Source { get; }
        Type Destination { get; }

        object Map(object source, NameValueCollection values = null);
    }
}