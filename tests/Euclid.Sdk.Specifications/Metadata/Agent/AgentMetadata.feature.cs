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
namespace Euclid.Sdk.Specifications.Metadata.Agent
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.7.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("In order to satisfy requests for metadata\r\nAs an agent\r\nI need to provide metadat" +
        "a in arbitrary formats")]
    [NUnit.Framework.CategoryAttribute("SdkSpecs")]
    [NUnit.Framework.CategoryAttribute("MetadataService")]
    [NUnit.Framework.CategoryAttribute("AgentMetadata")]
    public partial class EuclidAgentsProvideMetadataFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "AgentMetadata.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Euclid agents provide metadata", "In order to satisfy requests for metadata\r\nAs an agent\r\nI need to provide metadat" +
                    "a in arbitrary formats", ProgrammingLanguage.CSharp, new string[] {
                        "SdkSpecs",
                        "MetadataService",
                        "AgentMetadata"});
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
        [NUnit.Framework.DescriptionAttribute(": Agents have 2 types of metadata representations")]
        [NUnit.Framework.TestCaseAttribute("basic", "xml", "text/xml", new string[0])]
        [NUnit.Framework.TestCaseAttribute("basic", "json", "application/json", new string[0])]
        [NUnit.Framework.TestCaseAttribute("full", "xml", "text/xml", new string[0])]
        [NUnit.Framework.TestCaseAttribute("full", "json", "application/json", new string[0])]
        public virtual void AgentsHave2TypesOfMetadataRepresentations(string representation_Type, string format_Name, string content_Type, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo(": Agents have 2 types of metadata representations", exampleTags);
#line 7
this.ScenarioSetup(scenarioInfo);
#line 8
    testRunner.Given("an agent");
#line 9
    testRunner.When(string.Format("the {0} is requested", representation_Type));
#line 10
    testRunner.When(string.Format("metadata is requested as {0}", format_Name));
#line 11
    testRunner.Then(string.Format("it can be represented as {0}", content_Type));
#line 12
    testRunner.And("has been independently validated");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Agent parts provide formatted metadata")]
        [NUnit.Framework.TestCaseAttribute("command", "xml", "text/xml", new string[0])]
        [NUnit.Framework.TestCaseAttribute("command", "json", "application/json", new string[0])]
        [NUnit.Framework.TestCaseAttribute("query", "xml", "text/xml", new string[0])]
        [NUnit.Framework.TestCaseAttribute("query", "json", "application/json", new string[0])]
        [NUnit.Framework.TestCaseAttribute("readmodel", "xml", "text/xml", new string[0])]
        [NUnit.Framework.TestCaseAttribute("readmodel", "json", "application/json", new string[0])]
        public virtual void AgentPartsProvideFormattedMetadata(string agent_Part, string format_Name, string content_Type, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Agent parts provide formatted metadata", exampleTags);
#line 21
this.ScenarioSetup(scenarioInfo);
#line 22
 testRunner.Given(string.Format("the part {0}", agent_Part));
#line 23
 testRunner.When(string.Format("metadata is requested as {0}", format_Name));
#line 24
    testRunner.Then(string.Format("it can be represented as {0}", content_Type));
#line 25
    testRunner.And("has been independently validated");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Agent part collections provide formatted metadata")]
        [NUnit.Framework.TestCaseAttribute("commands", "xml", "text/xml", new string[0])]
        [NUnit.Framework.TestCaseAttribute("commands", "json", "application/json", new string[0])]
        [NUnit.Framework.TestCaseAttribute("queries", "xml", "text/xml", new string[0])]
        [NUnit.Framework.TestCaseAttribute("queries", "json", "application/json", new string[0])]
        [NUnit.Framework.TestCaseAttribute("readmodels", "xml", "text/xml", new string[0])]
        [NUnit.Framework.TestCaseAttribute("readmodels", "json", "application/json", new string[0])]
        public virtual void AgentPartCollectionsProvideFormattedMetadata(string descriptive_Name, string format_Name, string content_Type, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Agent part collections provide formatted metadata", exampleTags);
#line 39
this.ScenarioSetup(scenarioInfo);
#line 40
 testRunner.Given(string.Format("a part collection {0}", descriptive_Name));
#line 41
 testRunner.When(string.Format("metadata is requested as {0}", format_Name));
#line 42
    testRunner.Then(string.Format("it can be represented as {0}", content_Type));
#line 43
    testRunner.And("has been independently validated");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Collections of agent metadata provide 2 types of formatted metadata")]
        [NUnit.Framework.TestCaseAttribute("basic", "xml", "text/xml", new string[0])]
        [NUnit.Framework.TestCaseAttribute("basic", "json", "application/json", new string[0])]
        [NUnit.Framework.TestCaseAttribute("full", "xml", "text/xml", new string[0])]
        [NUnit.Framework.TestCaseAttribute("full", "json", "application/json", new string[0])]
        public virtual void CollectionsOfAgentMetadataProvide2TypesOfFormattedMetadata(string representation_Type, string format_Name, string content_Type, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Collections of agent metadata provide 2 types of formatted metadata", exampleTags);
#line 54
this.ScenarioSetup(scenarioInfo);
#line 55
    testRunner.Given("an agent collection");
#line 56
    testRunner.When(string.Format("the {0} is requested from the collection", representation_Type));
#line 57
    testRunner.When(string.Format("metadata is requested as {0}", format_Name));
#line 58
    testRunner.Then(string.Format("it can be represented as {0}", content_Type));
#line 59
    testRunner.And("has been independently validated");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#endregion
