using System;
using System.Collections.Generic;

namespace Euclid.Framework.AgentMetadata
{
	public interface IPartCollection
	{
		string AgentSystemName { get; }

		IEnumerable<IAgentPartMetadata> Collection { get; }
		Type CollectionType { get; }
		string DescriptiveName { get; }
		string Namespace { get; }
		IMetadataFormatter GetFormatter();
	}
}