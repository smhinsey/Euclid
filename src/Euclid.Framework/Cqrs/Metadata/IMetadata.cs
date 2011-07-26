using System;

namespace Euclid.Framework.Cqrs.Metadata
{
	public interface IMetadata
	{
		string Name { get; set; }
		string Namespace { get; }
		Type Type { get; set; }
	}
}