using System;

namespace Euclid.Framework.Agent.Metadata
{
	public interface IPropertyMetadata
	{
		string Name { get; set; }
		Type PropertyType { get; set; }
	}
}