using System;
using System.Collections.Generic;

namespace Euclid.Framework.Agent.Metadata
{
    public interface ITypeMetadata
	{
		IEnumerable<IInterfaceMetadata> Interfaces { get; }
		IEnumerable<IMethodMetadata> Methods { get; }
		string Name { get; set; }
		string Namespace { get; }

		IEnumerable<IPropertyMetadata> Properties { get; }
		Type Type { get; set; }

		IEnumerable<IPropertyMetadata> GetAttributes(Type type);
	}
}