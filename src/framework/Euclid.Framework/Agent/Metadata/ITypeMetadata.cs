using System;
using System.Collections.Generic;

namespace Euclid.Framework.Agent.Metadata
{
    public interface ITypeMetadata
    {
        string Name { get; set; }
        string Namespace { get; }

        Type Type { get; set; }

        IEnumerable<IMethodMetadata> Methods { get; }
        IEnumerable<IPropertyMetadata> Properties { get; }
        IMetadataFormatter GetFormatter();
        IPartCollection GetContainingPartCollection();
    }
}