using System;
using System.Collections.Generic;

namespace Euclid.Framework.Agent.Metadata
{
	public interface IAgentPartMetadataCollection : IList<ITypeMetadata>
	{
		string AgentSystemName { get; }
		string Namespace { get; }

		ITypeMetadata GetMetadata<TImplementationType>() where TImplementationType : IAgentPart;
		ITypeMetadata GetMetadata(Type agentPartImplementationType);
		ITypeMetadata GetMetadata(string agentPartImplementationName);

		bool Registered<TImplementationType>();
		bool Registered(Type agentPartImplementationType);
		bool Registered(string agentPartImplementationName);
	}
}