using System;
using System.Linq;
using Euclid.Common.Configuration;
using Euclid.Composites;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Euclid.Sdk.Specifications.AppSettings
{
    [Binding]
    public class CompositeAppSettingsCanBeValidated
    {
        [Given("A new CompositeAppSetting object")]
        public void NewCompositeAppSetting()
        {
            ScenarioContext.Current["AppSetting"] = new CompositeAppSettings();
        }

        [When("I call validate a NullSettingException is thrown")]
        public void ValidateDefaultCompositeAppSettings()
        {
            var setting = ScenarioContext.Current["AppSetting"] as CompositeAppSettings;

            Assert.NotNull(setting);

            try
            {
                setting.Validate();
                Assert.Fail("default composite app should not be valid");
            }
            catch (NullSettingException n)
            {
                ScenarioContext.Current["NullSettingName"] = n.SettingName;
            }
            catch
            {
                Assert.Fail("default composite app should throw NullSettingException");
            }
        }

        [When("NullSettingException.SettingName is equal to 'OutputChannel'")]
        public void SettingNameIsOutputChannel()
        {
            var settingName = ScenarioContext.Current["NullSettingName"].ToString();

            Assert.False(string.IsNullOrEmpty(settingName));

            Assert.AreEqual("OutputChannel", settingName);
        }

        [When(@"There is 1 reason in the enumerable object returned by CompositeAppSetting\.GetInvalidSettingReasons\(\)")]
        public void TestInvalidSettingReasons()
        {
            var setting = ScenarioContext.Current["AppSetting"] as CompositeAppSettings;

            Assert.NotNull(setting);

            var reasons = setting.GetInvalidSettingReasons();

            Console.WriteLine(reasons.ElementAt(0));

            Assert.AreEqual(1, reasons.Count());
        }
    }
}