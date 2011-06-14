using System.Collections.Generic;
using System.Reflection;
using Euclid.Common.Configuration;
using Euclid.Common.TestingFakes.Configuration;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Configuration
{
	public class OverridableObjectsTests
	{
		[Test]
		public void BuildBasicSettings()
		{
			const string fakeSettingDefaultValue = "Default value";
			const bool anotherFakeSettingDefaultValue = true;

			var config = OverridableSettings<FakeSettings>
				.Build(settings =>
				       	{
				       		settings.FakeConfigSetting.WithDefault(fakeSettingDefaultValue);
				       		settings.AnotherFakeConfigSetting.WithDefault(anotherFakeSettingDefaultValue);
				       	});

			Assert.AreEqual(fakeSettingDefaultValue, config.FakeConfigSetting.Value);
			Assert.AreEqual(anotherFakeSettingDefaultValue, config.AnotherFakeConfigSetting.Value);
		}

		[Test]
		public void BuildBasicSettingsWithAppSettingOverride()
		{
			const string fakeSettingDefaultValue = "Default value";
			const string fakeSettingOverriddenValue = "Overridden value";
			const bool anotherFakeSettingDefaultValue = true;

			var config = OverridableSettings<FakeSettings>
				.Build(settings =>
				{
					settings.FakeConfigSetting.WithDefault(fakeSettingDefaultValue);
					settings.AnotherFakeConfigSetting.WithDefault(anotherFakeSettingDefaultValue);
				});

			config.OverrideFromAppSettings();

			Assert.AreEqual(fakeSettingOverriddenValue, config.FakeConfigSetting.Value);
			Assert.AreEqual(anotherFakeSettingDefaultValue, config.AnotherFakeConfigSetting.Value);
		}

		[Test]
		public void BuildSettingsWithList()
		{
			const string fakeSettingDefaultValue = "Default value";
			const bool anotherFakeSettingDefaultValue = true;

			var config = OverridableSettings<FakeSettings>
				.Build(settings =>
				       	{
				       		settings.FakeConfigSetting.WithDefault(fakeSettingDefaultValue);
				       		settings.AnotherFakeConfigSetting.WithDefault(
				       		                                              anotherFakeSettingDefaultValue);
				       		settings.ListOfAssemblies.WithDefault(new List<Assembly>());
				       		settings.ListOfAssemblies.Add(GetType().Assembly);
				       	});

			Assert.AreEqual(fakeSettingDefaultValue, config.FakeConfigSetting.Value);
			Assert.AreEqual(anotherFakeSettingDefaultValue, config.AnotherFakeConfigSetting.Value);
			Assert.AreEqual(1, config.ListOfAssemblies.Value.Count);
		}
	}
}