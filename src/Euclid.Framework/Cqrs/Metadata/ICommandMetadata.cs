using System.Collections.Generic;

namespace Euclid.Framework.Cqrs.Metadata
{
	public interface ICommandMetadata : IMetadata
	{
		IList<IMetadata> Interfaces { get; }
		IList<IPropertyMetadata> Properties { get; }
	}
}