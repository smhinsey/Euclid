namespace Euclid.Common.Configuration
{
	public interface IOverridableSetting<TSettingType>
	{
		TSettingType DefaultValue { get; }
		TSettingType Value { get; }
		bool WasOverridden { get; }
		void WithDefault(TSettingType value);
		void ApplyOverride(TSettingType newValue);
	}
}