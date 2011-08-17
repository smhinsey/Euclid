using System;

namespace Euclid.Framework.AgentMetadata
{
	public class ArgumentMetadata : IArgumentMetadata
	{
		public object DefaultValue { get; set; }
		public string Name { get; set; }
		public int Order { get; set; }
		public Type PropertyType { get; set; }
	}
}