// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.7.0.0
//      SpecFlow Generator Version:1.7.0.0
//      Runtime Version:4.0.30319.225
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
namespace Euclid.Sdk.Metadata
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.7.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("In order to fail fast and identify configuration errors early\r\nAs a composite dev" +
        "eloper\r\nI want to be able to validate configuration and get a description of any" +
        " errors that occur")]
    public partial class CompositeSettingsCanBeValidatedFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "CompositeSettingsCanBeValidated.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Composite settings can be validated", "In order to fail fast and identify configuration errors early\r\nAs a composite dev" +
                    "eloper\r\nI want to be able to validate configuration and get a description of any" +
                    " errors that occur", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Validating default CompostiteAppSetting fails")]
        [NUnit.Framework.CategoryAttribute("configuration")]
        [NUnit.Framework.CategoryAttribute("composite")]
        public virtual void ValidatingDefaultCompostiteAppSettingFails()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Validating default CompostiteAppSetting fails", new string[] {
                        "configuration",
                        "composite"});
#line 7
this.ScenarioSetup(scenarioInfo);
#line 8
 testRunner.Given("A new CompositeAppSetting object");
#line 9
 testRunner.When("I call validate a NullSettingException is thrown");
#line 10
    testRunner.And("NullSettingException.SettingName is equal to \'OutputChannel\'");
#line 11
    testRunner.And("There is 1 reason in the enumerable object returned by CompositeAppSetting.GetInv" +
                    "alidSettingReasons()");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Setting an OutputChannel on the CompositeAppSetting")]
        public virtual void SettingAnOutputChannelOnTheCompositeAppSetting()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Setting an OutputChannel on the CompositeAppSetting", ((string[])(null)));
#line 13
this.ScenarioSetup(scenarioInfo);
#line 14
    testRunner.Given("A new CompositeAppSetting object");
#line 15
    testRunner.When("I apply an InMemoryMessageChannel to the OutputChannel property");
#line 16
    testRunner.And("I call validate no exceptions are thrown");
#line 17
    testRunner.And("CompositeAppSetting.GetInvalidSettingReasons() returns 0 length enumerable object" +
                    "");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#endregion
