using System;
using System.Collections.Generic;

namespace Euclid.Framework.Agent.Metadata
{
	public interface ITypeMetadata
	{
		IList<IInterfaceMetadata> Interfaces { get; }
		string Name { get; set; }
		string Namespace { get; }

		IList<IPropertyMetadata> Properties { get; }
		Type Type { get; set; }

		IList<IPropertyMetadata> GetAttributes(Type type);
	}
}