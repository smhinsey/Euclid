using System.Collections.Generic;
using System.Reflection;
using Euclid.Framework.Cqrs.Metadata;

namespace Euclid.Composites.Metadata
{
	public class PropertyMetadata : Metadata, IPropertyMetadata
	{
		public PropertyMetadata(PropertyInfo propertyInfo) : base(propertyInfo.PropertyType)
		{
			Name = propertyInfo.Name;

			CustomAttributes = new List<IMetadata>();

			foreach (var attr in propertyInfo.GetCustomAttributes(true))
			{
				CustomAttributes.Add(Extract(attr.GetType()));
			}
		}

		public IList<IMetadata> CustomAttributes { get; private set; }
		public object Value { get; set; }

		internal static IPropertyMetadata Extract(PropertyInfo propertyInfo)
		{
			return new PropertyMetadata(propertyInfo);
		}
	}
}