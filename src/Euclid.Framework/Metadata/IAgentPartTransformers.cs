using System;

namespace Euclid.Framework.Metadata
{
    public interface  IAgentPartTransformers
    {
        TDestination Transform<TDestination>(Type partImplementation);
    }
}