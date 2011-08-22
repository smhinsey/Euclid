namespace Euclid.Common.Configuration
{
	public class OverridableSetting<TSettingType> : IOverridableSetting<TSettingType>
	{
		public TSettingType DefaultValue { get; private set; }

		public TSettingType Value { get; private set; }

		public bool WasOverridden { get; private set; }

		public void ApplyOverride(TSettingType newValue)
		{
			this.Value = newValue;
			this.WasOverridden = true;
		}

		public void WithDefault(TSettingType value)
		{
			this.DefaultValue = value;
			this.Value = value;
		}
	}
}