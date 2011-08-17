using System;
using System.Collections;
using System.Collections.Generic;

namespace Euclid.Framework.Agent.Metadata
{
    public interface IAgentPartMetadataCollection : IList<ITypeMetadata>
    {
        
    }

    public interface IAgentPartMetadataFormatterCollection : IMetadataFormatter, IAgentPartMetadataCollection
    {

		ITypeMetadata GetMetadata(Type agentPartImplementationType);
		bool Registered(Type agentPartImplementationType);
	}
}