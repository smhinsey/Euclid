using System;

namespace Euclid.Common.Configuration
{
	public class OverridableSettings<TOverridableSettings>
		where TOverridableSettings : IOverridableSettings, new()
	{
		static OverridableSettings()
		{
			Settings = new TOverridableSettings();
		}

		public static TOverridableSettings Settings { get; set; }

		public static TOverridableSettings Build(Action<TOverridableSettings> action)

		{
			action(Settings);

			return Settings;
		}
	}
}