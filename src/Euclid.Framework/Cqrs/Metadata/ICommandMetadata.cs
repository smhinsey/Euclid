using System.Collections.Generic;

namespace Euclid.Framework.Cqrs.Metadata
{
    public interface ICommandMetadata : IMetadata
    {
        IList<IPropertyMetadata> Properties { get; }
        IList<IMetadata> Interfaces { get; }
    }
}