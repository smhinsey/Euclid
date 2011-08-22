namespace Euclid.Common.Configuration
{
	public interface IOverridableSetting<TSettingType>
	{
		TSettingType DefaultValue { get; }

		TSettingType Value { get; }

		bool WasOverridden { get; }

		void ApplyOverride(TSettingType newValue);

		void WithDefault(TSettingType value);
	}
}