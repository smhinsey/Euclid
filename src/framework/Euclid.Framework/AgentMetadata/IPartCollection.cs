using System;
using System.Collections.Generic;

namespace Euclid.Framework.AgentMetadata
{
	public interface IPartCollection : IEnumerable<IPartMetadata>
	{
		string AgentSystemName { get; }

		Type CollectionType { get; }

		string DescriptiveName { get; }

		string Namespace { get; }

		IMetadataFormatter GetFormatter();
	}
}