using System.Collections.Generic;

namespace Euclid.Common.Configuration
{
	public class OverridableSettingList<TSettingType> : IOverridableSettingList<TSettingType>
	{
		public OverridableSettingList()
		{
			this.DefaultValue = new List<TSettingType>();
			this.Value = new List<TSettingType>();
		}

		public IList<TSettingType> DefaultValue { get; private set; }

		public IList<TSettingType> Value { get; private set; }

		public bool WasOverridden { get; private set; }

		public void Add(TSettingType newListItem)
		{
			this.Value.Add(newListItem);
		}

		public void ApplyOverride(IList<TSettingType> newValue)
		{
			this.Value = newValue;
			this.WasOverridden = true;
		}

		public void WithDefault(IList<TSettingType> value)
		{
			this.DefaultValue = value;
			this.Value = value;
		}
	}
}