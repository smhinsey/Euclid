using System;
using System.Reflection;

namespace Euclid.Framework.Agent.Metadata
{
    public class PropertyMetadataProvider : IPropertyMetadata
	{
		public PropertyMetadataProvider()
		{
		}

		public PropertyMetadataProvider(PropertyInfo pi)
		{
			Name = pi.Name;
			PropertyType = pi.PropertyType;
		}

		public bool IsWritable { get; set; }

		public string Name { get; set; }
		public Type PropertyType { get; set; }
	}
}