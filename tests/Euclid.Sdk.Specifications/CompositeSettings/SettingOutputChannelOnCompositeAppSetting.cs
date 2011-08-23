using System.Linq;
using Euclid.Common.Messaging;
using Euclid.Composites;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Euclid.Sdk.Metadata.CompositeSettings
{
    [Binding]
    public class SettingOutputChannelOnCompositeAppSetting
    {
        [When("I apply an InMemoryMessageChannel to the OutputChannel property")]
        public void ApplyMessageChannel()
        {
            var setting = ScenarioContext.Current["AppSetting"] as CompositeAppSettings;

            Assert.NotNull(setting);

            setting.OutputChannel.ApplyOverride(typeof(InMemoryMessageChannel));
        }

        [When("I call validate no exceptions are thrown")]
        public void ValidateSettings()
        {
            var setting = ScenarioContext.Current["AppSetting"] as CompositeAppSettings;

            Assert.NotNull(setting);

            setting.Validate();
        }

        [When(@"CompositeAppSetting\.GetInvalidSettingReasons\(\) returns 0 length enumerable object")]
        public void SettingsAreValid()
        {
            var setting = ScenarioContext.Current["AppSetting"] as CompositeAppSettings;

            Assert.NotNull(setting);

            var reasons = setting.GetInvalidSettingReasons();

            Assert.AreEqual(0, reasons.Count());
        }
    }
}