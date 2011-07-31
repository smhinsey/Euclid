using System;

namespace Euclid.Framework.Metadata
{
	public interface IMetadataOriginal
	{
		string Name { get; set; }
		string Namespace { get; }
		Type Type { get; set; }
	}
}