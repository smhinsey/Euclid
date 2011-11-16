// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.7.0.0
//      SpecFlow Generator Version:1.7.0.0
//      Runtime Version:4.0.30319.239
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------

#region Designer generated code

using TechTalk.SpecFlow;

namespace Euclid.Sdk.Specifications.Metadata.Composite
{
	[System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.7.0.0")]
	[System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
	[NUnit.Framework.TestFixtureAttribute()]
	[NUnit.Framework.DescriptionAttribute(
		"In order to satisfy requests for metadata\r\nAs a composite\r\nI need to provide meta" + "data in arbitrary formats")
	]
	[NUnit.Framework.CategoryAttribute("SdkSpecs")]
	[NUnit.Framework.CategoryAttribute("MetadataService")]
	[NUnit.Framework.CategoryAttribute("CompositeMetadata")]
	public partial class CompositeProvidesConfigurationMetadataFeature
	{
		private static TechTalk.SpecFlow.ITestRunner testRunner;

#line 1 "CompositeMetadata.feature"
#line hidden

		[NUnit.Framework.TestFixtureSetUpAttribute()]
		public virtual void FeatureSetup()
		{
			testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
			var featureInfo = new TechTalk.SpecFlow.FeatureInfo(
				new System.Globalization.CultureInfo("en-US"),
				"Composite provides configuration metadata",
				"In order to satisfy requests for metadata\r\nAs a composite\r\nI need to provide meta"
				+ "data in arbitrary formats",
				ProgrammingLanguage.CSharp,
				new string[] { "SdkSpecs", "MetadataService", "CompositeMetadata" });
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
		[NUnit.Framework.DescriptionAttribute("Composite can provide metadata about it\'s configuration")]
		[NUnit.Framework.TestCaseAttribute("is", "true", "0", new string[0])]
		[NUnit.Framework.TestCaseAttribute("isn\'t", "false", "1", new string[0])]
		public virtual void CompositeCanProvideMetadataAboutItSConfiguration(
			string is_Or_IsnT, string true_Or_False, string item_Count, string[] exampleTags)
		{
			var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo(
				"Composite can provide metadata about it\'s configuration", exampleTags);
#line 7
			this.ScenarioSetup(scenarioInfo);
#line 8
			testRunner.Given(string.Format("a composite that {0} configured", is_Or_IsnT));
#line 9
			testRunner.When("I call IsValid");
#line 10
			testRunner.Then(string.Format("the result should be {0}", true_Or_False));
#line 11
			testRunner.And(
				string.Format(
					"the call to GetConfigurationErrors returns an enumerable list that contains {0} i" + "tems", item_Count));
#line hidden
			this.ScenarioCleanup();
		}

		[NUnit.Framework.TestAttribute()]
		[NUnit.Framework.DescriptionAttribute("Composite provides formatted metadata")]
		[NUnit.Framework.TestCaseAttribute("is", "xml", "text/xml", new string[0])]
		[NUnit.Framework.TestCaseAttribute("is", "json", "application/json", new string[0])]
		[NUnit.Framework.TestCaseAttribute("isn\'t", "xml", "text/xml", new string[0])]
		[NUnit.Framework.TestCaseAttribute("isn\'t", "json", "application/json", new string[0])]
		public virtual void CompositeProvidesFormattedMetadata(
			string is_Or_IsnT, string format_Name, string content_Type, string[] exampleTags)
		{
			var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Composite provides formatted metadata", exampleTags);
#line 19
			this.ScenarioSetup(scenarioInfo);
#line 20
			testRunner.Given(string.Format("a composite that {0} configured", is_Or_IsnT));
#line 21
			testRunner.And("it contains the TestAgent");
#line 22
			testRunner.And("it contains the TestInputModel");
#line 23
			testRunner.When(string.Format("metadata is requested as {0}", format_Name));
#line 24
			testRunner.Then(string.Format("it can be represented as {0}", content_Type));
#line 25
			testRunner.And("has been independently validated");
#line hidden
			this.ScenarioCleanup();
		}
	}
}

#endregion