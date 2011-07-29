using System;
using System.Collections.Generic;

namespace Euclid.Framework.Cqrs.Metadata
{
    public interface ICommandMetadata
    {
        string Name { get; set; }
        Type Type { get; set; }
        string Namespace { get; }
        IList<IPropertyMetadata> Properties { get; }
        IList<IMetadata> Interfaces { get; }
    }
}