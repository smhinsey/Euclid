using System;
using System.Reflection;

namespace Euclid.Framework.Agent.Metadata
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

		public string Name { get; set; }
		public Type PropertyType { get; set; }
        public bool IsWritable { get; set; }
	}
}