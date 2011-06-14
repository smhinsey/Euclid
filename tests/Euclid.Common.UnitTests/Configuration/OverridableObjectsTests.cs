﻿using System.Collections.Generic;
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
			const FakeSettingModes enumSetting = FakeSettingModes.ModeTwo;
			const int numericSetting = 15;

			var config = OverridableSettings<FakeSettings>
				.Build(settings =>
				       	{
				       		settings.FakeConfigSetting.WithDefault(fakeSettingDefaultValue);
									settings.AnotherFakeConfigSetting.WithDefault(anotherFakeSettingDefaultValue);
				       		settings.NumericConfigSetting.WithDefault(numericSetting);
				       		settings.EnumConfigSetting.WithDefault(enumSetting);
				       	});

			Assert.AreEqual(fakeSettingDefaultValue, config.FakeConfigSetting.Value);
			Assert.AreEqual(anotherFakeSettingDefaultValue, config.AnotherFakeConfigSetting.Value);
			Assert.AreEqual(enumSetting, config.EnumConfigSetting.Value);
			Assert.AreEqual(numericSetting, config.NumericConfigSetting.Value);
		}

		[Test]
		public void BuildBasicSettingsWithAppSettingOverride()
		{
			const string fakeSettingDefaultValue = "Default value";
			const string fakeSettingOverriddenValue = "Overridden value";
			const bool anotherFakeSettingDefaultValue = true;
			const FakeSettingModes enumSetting = FakeSettingModes.ModeTwo;
			const int numericSetting = 15;
			const int overriddenNumericSetting = 12;

			var config = OverridableSettings<FakeSettings>
				.Build(settings =>
				{
					settings.FakeConfigSetting.WithDefault(fakeSettingDefaultValue);
					settings.AnotherFakeConfigSetting.WithDefault(anotherFakeSettingDefaultValue);
					settings.NumericConfigSetting.WithDefault(numericSetting);
					settings.EnumConfigSetting.WithDefault(enumSetting);
				});

			config.OverrideFromAppSettings();

			Assert.AreEqual(fakeSettingOverriddenValue, config.FakeConfigSetting.Value);
			Assert.AreEqual(anotherFakeSettingDefaultValue, config.AnotherFakeConfigSetting.Value);
			Assert.AreEqual(enumSetting, config.EnumConfigSetting.Value);
			Assert.AreEqual(overriddenNumericSetting, config.NumericConfigSetting.Value);
		}

		[Test]
		public void BuildSettingsWithList()
		{
			const string fakeSettingDefaultValue = "Default value";
			const bool anotherFakeSettingDefaultValue = true;
			var currentAssembly = GetType().Assembly;

			var config = OverridableSettings<FakeSettings>
				.Build(settings =>
				       	{
				       		settings.FakeConfigSetting.WithDefault(fakeSettingDefaultValue);
				       		settings.AnotherFakeConfigSetting.WithDefault(
				       		                                              anotherFakeSettingDefaultValue);
				       		settings.ListOfAssemblies.WithDefault(new List<Assembly>());
				       		settings.ListOfAssemblies.Add(currentAssembly);
				       	});

			Assert.AreEqual(fakeSettingDefaultValue, config.FakeConfigSetting.Value);
			Assert.AreEqual(anotherFakeSettingDefaultValue, config.AnotherFakeConfigSetting.Value);
			Assert.AreEqual(1, config.ListOfAssemblies.Value.Count);
			Assert.AreEqual(currentAssembly, config.ListOfAssemblies.Value[0]);
		}
	}
}