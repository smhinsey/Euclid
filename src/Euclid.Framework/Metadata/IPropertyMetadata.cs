using System;

namespace Euclid.Framework.Metadata
{
	public interface IPropertyMetadata
	{
		string Name { get; set; }
		int Order { get; set; }
		Type PropertyType { get; set; }
		Type PropertyValueSetterType { get; set; }
	}
}