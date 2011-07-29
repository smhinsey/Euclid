using System.Collections.Generic;

namespace Euclid.Framework.Cqrs.Metadata
{
	public interface IPropertyMetadata : IMetadata
	{
		IList<IMetadata> CustomAttributes { get; }
	}
}