using System;
using System.Collections.Generic;
using System.Linq;

namespace Euclid.Framework.Metadata
{
    public interface IAgentPartTransformer<TAgentPart, out TDestination> : IAgentPartTransformer where TAgentPart : IAgentPart
    {
        IList<TAgentPart> ImplementationTypes { get; }
        TDestination Transform(TAgentPart implementation);
    }

    public interface IAgentPartTransformer
    {
        //marker
        Type SourceType { get; }
        Type DestinationType { get; }

        object Transform(Type implementation);
    }
}