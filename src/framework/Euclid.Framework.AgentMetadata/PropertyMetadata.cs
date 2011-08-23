using System;
using System.Reflection;

namespace Euclid.Framework.AgentMetadata
{
	public class PropertyMetadata : IPropertyMetadata
	{
		public PropertyMetadata()
		{
		}

		public PropertyMetadata(PropertyInfo pi)
		{
			Name = pi.Name;
			PropertyType = pi.PropertyType;
		}

		public bool IsWritable { get; set; }

		public string Name { get; set; }

		public Type PropertyType { get; set; }
	}
}