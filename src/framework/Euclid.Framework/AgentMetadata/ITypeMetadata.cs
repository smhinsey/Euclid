using System;
using System.Collections.Generic;

namespace Euclid.Framework.AgentMetadata
{
	public interface ITypeMetadata
	{
		IEnumerable<IMethodMetadata> Methods { get; }

		string Name { get; set; }

		string Namespace { get; }

		IEnumerable<IPropertyMetadata> Properties { get; }

		Type Type { get; set; }

		IPartCollection GetContainingPartCollection();

		IMetadataFormatter GetFormatter();
	}
}