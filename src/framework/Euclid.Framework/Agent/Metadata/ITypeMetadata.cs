using System;
using System.Collections.Generic;

namespace Euclid.Framework.Agent.Metadata
{
    public interface ITypeMetadata
	{
        string Name { get; set; }
        string Namespace { get; }
        
        Type Type { get; set; }

        IEnumerable<IInterfaceMetadata> Interfaces { get; }
		IEnumerable<IMethodMetadata> Methods { get; }
        IEnumerable<IPropertyMetadata> Properties { get; }
        IEnumerable<IPropertyMetadata> GetAttributes(Type type);

        IMetadataFormatter GetFormatter();
	}
}