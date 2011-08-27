namespace Euclid.Common.Configuration
{
	/// <summary>
	/// 	An individual overridable setting.
	/// </summary>
	/// <typeparam name = "TSettingType">The setting's type.</typeparam>
	public interface IOverridableSetting<TSettingType>
	{
		/// <summary>
		/// 	Gets default value of the setting.
		/// </summary>
		TSettingType DefaultValue { get; }

		/// <summary>
		/// 	Gets the value of the setting.
		/// </summary>
		TSettingType Value { get; }

		/// <summary>
		/// 	Gets a value indicating whether or not the DefaultValue was overriden.
		/// </summary>
		bool WasOverridden { get; }

		/// <summary>
		/// 	Applies a new Value to the setting.
		/// </summary>
		/// <param name = "newValue">The new value.</param>
		void ApplyOverride(TSettingType newValue);

		/// <summary>
		/// 	Creates an initial default value for the setting.
		/// </summary>
		/// <param name = "value">The value to set.</param>
		void WithDefault(TSettingType value);
	}
}