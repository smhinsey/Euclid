using System.Collections.Generic;

namespace Euclid.Framework.Metadata
{
	public interface IPropertyMetadata : IMetadataOriginal
	{
		IList<IMetadataOriginal> CustomAttributes { get; }
	}
}