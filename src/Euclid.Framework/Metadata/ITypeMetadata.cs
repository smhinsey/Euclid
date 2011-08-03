using System;
using System.Collections.Generic;

namespace Euclid.Framework.Metadata
{
    public interface ITypeMetadata
    {
        Type Type { get; set; }
        string Name { get; set; }
        string Namespace { get; }

        IList<IPropertyMetadata> Properties { get; }
        IList<IInterfaceMetadata> Interfaces { get; }

        IList<IPropertyMetadata> GetAttributes(Type type);
    }
}