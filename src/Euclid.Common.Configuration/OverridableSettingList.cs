using System.Collections.Generic;

namespace Euclid.Common.Configuration
{
	public class OverridableSettingList<TSettingType> : IOverridableSettingList<TSettingType>
	{
		public IList<TSettingType> DefaultValue { get; private set; }
		public IList<TSettingType> Value { get; private set; }
		public bool WasOverridden { get; private set; }

		public void Add(TSettingType newListItem)
		{
			Value.Add(newListItem);
		}

		public void ApplyOverride(IList<TSettingType> newValue)
		{
			Value = newValue;
			WasOverridden = true;
		}

		public void WithDefault(IList<TSettingType> value)
		{
			DefaultValue = value;
			Value = value;
		}
	}
}