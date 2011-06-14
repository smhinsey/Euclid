namespace Euclid.Common.Configuration
{
	public class OverridableSetting<TSettingType> : IOverridableSetting<TSettingType>
	{
		public TSettingType DefaultValue { get; private set; }
		public TSettingType Value { get; private set; }
		public bool WasOverridden { get; private set; }

		public void WithDefault(TSettingType value)
		{
			DefaultValue = value;
			Value = value;
		}

		public void ApplyOverride(TSettingType newValue)
		{
			Value = newValue;
			WasOverridden = true;
		}
	}
}