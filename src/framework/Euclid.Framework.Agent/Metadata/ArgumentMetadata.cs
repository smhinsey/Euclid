using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Euclid.Framework.Agent.Metadata
{
	public class ArgumentMetadata : IArgumentMetadata
	{
		public object DefaultValue { get; set; }
		public string Name { get; set; }
		public int Order { get; set; }
		public Type PropertyType { get; set; }
	}
}