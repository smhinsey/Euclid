using System.Reflection;
using Euclid.Common.Configuration;

namespace Euclid.Common.TestingFakes.Configuration
{
	public class FakeSettings : IOverridableSettings
	{
		public FakeSettings()
		{
			this.AnotherFakeConfigSetting = new OverridableSetting<bool>();
			this.FakeConfigSetting = new OverridableSetting<string>();
			this.ListOfAssemblies = new OverridableSettingList<Assembly>();
			this.NumericConfigSetting = new OverridableSetting<int>();
			this.EnumConfigSetting = new OverridableSetting<FakeSettingModes>();
		}

		public OverridableSetting<bool> AnotherFakeConfigSetting { get; set; }

		public OverridableSetting<FakeSettingModes> EnumConfigSetting { get; set; }

		public OverridableSetting<string> FakeConfigSetting { get; set; }

		public OverridableSettingList<Assembly> ListOfAssemblies { get; set; }

		public OverridableSetting<int> NumericConfigSetting { get; set; }
	}
}