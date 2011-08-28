using System.Configuration;

namespace Euclid.Common.Configuration
{
	public static class OverrideSettingsFromAppSettings
	{
		public static IOverridableSettings OverrideFromAppSettings(this IOverridableSettings settings)
		{
			foreach (var propertyInfo in settings.GetType().GetProperties())
			{
				var name = string.Format("{1}.{0}", propertyInfo.Name, settings.GetType().Name);
				var newValue = ConfigurationManager.AppSettings[name];

				if (newValue != null)
				{
					var propertyReference = propertyInfo.GetValue(settings, null);

					if (propertyReference is IOverridableSetting<string>)
					{
						((IOverridableSetting<string>)propertyReference).ApplyOverride(newValue);
					}
					else if (propertyReference is IOverridableSetting<bool>)
					{
						((IOverridableSetting<bool>)propertyReference).ApplyOverride(bool.Parse(newValue));
					}
					else if (propertyReference is IOverridableSetting<int>)
					{
						((IOverridableSetting<int>)propertyReference).ApplyOverride(int.Parse(newValue));
					}

					propertyInfo.SetValue(settings, propertyReference, null);
				}
			}

			return settings;
		}
	}
}