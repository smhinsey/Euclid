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
		}

		public OverridableSetting<bool> AnotherFakeConfigSetting { get; set; }
		public OverridableSetting<string> FakeConfigSetting { get; set; }
		public OverridableSettingList<Assembly> ListOfAssemblies { get; set; }
	}
}