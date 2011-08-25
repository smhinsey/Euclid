using System.Reflection;
using Euclid.Common.Configuration;

namespace Euclid.Common.TestingFakes.Configuration
{
	public class FakeSettings : IOverridableSettings
	{
		public FakeSettings()
		{
			AnotherFakeConfigSetting = new OverridableSetting<bool>();
			FakeConfigSetting = new OverridableSetting<string>();
			ListOfAssemblies = new OverridableSettingList<Assembly>();
			NumericConfigSetting = new OverridableSetting<int>();
			EnumConfigSetting = new OverridableSetting<FakeSettingModes>();
		}

		public OverridableSetting<bool> AnotherFakeConfigSetting { get; set; }

		public OverridableSetting<FakeSettingModes> EnumConfigSetting { get; set; }

		public OverridableSetting<string> FakeConfigSetting { get; set; }

		public OverridableSettingList<Assembly> ListOfAssemblies { get; set; }

		public OverridableSetting<int> NumericConfigSetting { get; set; }
	}
}
