// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.7.0.0
//      SpecFlow Generator Version:1.7.0.0
//      Runtime Version:4.0.30319.235
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
    [NUnit.Framework.DescriptionAttribute("In order to satisfy requests for metadata\r\nAs an agent\r\nI need to provide metadat" +
        "a in arbitrary formats")]
    [NUnit.Framework.CategoryAttribute("Feature")]
    public partial class EuclidAgentsProvideMetadataAboutTheirPartsFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "AgentPartsProvideMetadataTestSteps.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Euclid agents provide metadata about their parts", "In order to satisfy requests for metadata\r\nAs an agent\r\nI need to provide metadat" +
                    "a in arbitrary formats", ProgrammingLanguage.CSharp, new string[] {
                        "Feature"});
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
        [NUnit.Framework.DescriptionAttribute("Supported Agent metadata formats")]
        [NUnit.Framework.TestCaseAttribute("xml", "text/xml", new string[0])]
        [NUnit.Framework.TestCaseAttribute("json", "application/json", new string[0])]
        public virtual void SupportedAgentMetadataFormats(string format_Name, string content_Type, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Supported Agent metadata formats", exampleTags);
#line 7
this.ScenarioSetup(scenarioInfo);
#line 8
 testRunner.Given("an agent");
#line 9
 testRunner.When(string.Format("metadata is requested as {0}", format_Name));
#line 10
    testRunner.Then(string.Format("can be represented as {0}", content_Type));
#line 11
    testRunner.And("has been independently validated");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#endregion
