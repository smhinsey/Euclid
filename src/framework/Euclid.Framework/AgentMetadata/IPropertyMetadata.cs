using System;

namespace Euclid.Framework.AgentMetadata
{
	public interface IPropertyMetadata
	{
		string Name { get; set; }
		Type PropertyType { get; set; }
	}
}