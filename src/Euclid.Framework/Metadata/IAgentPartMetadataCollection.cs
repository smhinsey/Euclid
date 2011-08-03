using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Euclid.Framework.Metadata
{
    public interface IAgentPartMetadataCollection : IList<ITypeMetadata>
    {
        string Namespace { get; }
        string AgentSystemName { get; }

        ITypeMetadata GetMetadata<TImplementationType>() where TImplementationType: IAgentPart;
        ITypeMetadata GetMetadata(Type agentPartImplementationType);
        ITypeMetadata GetMetadata(string agentPartImplementationName);

        bool Registered<TImplementationType>();
        bool Registered(Type agentPartImplementationType);
        bool Registered(string agentPartImplementationName);
    }
}