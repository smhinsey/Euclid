using System;

namespace Euclid.Common.Configuration
{
	public class NullSettingException : Exception
	{
		public NullSettingException(string settingName)
			: base(string.Format("The setting '{0}' is null", settingName))
		{
			SettingName = settingName;
		}

		public string SettingName { get; private set; }
	}
}