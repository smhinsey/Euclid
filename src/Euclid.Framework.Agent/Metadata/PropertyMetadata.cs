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

		public PropertyMetadata(string name, Type type)
		{
			Name = name;
			PropertyType = type;
		}

		public string Name { get; set; }
		public int Order { get; set; }
		public Type PropertyType { get; set; }
		public Type PropertyValueSetterType { get; set; }
	}
}