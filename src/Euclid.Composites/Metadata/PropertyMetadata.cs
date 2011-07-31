using System.Collections.Generic;
using System.Reflection;
using Euclid.Framework.Metadata;

namespace Euclid.Composites.Metadata
{
	public class PropertyMetadata : Metadata, IPropertyMetadata
	{
		public PropertyMetadata(PropertyInfo propertyInfo) : base(propertyInfo.PropertyType)
		{
			Name = propertyInfo.Name;

			CustomAttributes = new List<IMetadataOriginal>();

			foreach (var attr in propertyInfo.GetCustomAttributes(true))
			{
				CustomAttributes.Add(Extract(attr.GetType()));
			}
		}

		public IList<IMetadataOriginal> CustomAttributes { get; private set; }
		public object Value { get; set; }

		internal static IPropertyMetadata Extract(PropertyInfo propertyInfo)
		{
			return new PropertyMetadata(propertyInfo);
		}
	}
}