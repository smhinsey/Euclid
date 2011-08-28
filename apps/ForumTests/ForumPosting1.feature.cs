// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.7.0.0
//      SpecFlow Generator Version:1.7.0.0
//      Runtime Version:4.0.30319.235
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------

#region Designer generated code

using System.Globalization;
using TechTalk.SpecFlow;

namespace ForumTests
{
	[System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.7.0.0")]
	[System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
	[NUnit.Framework.TestFixtureAttribute()]
	[NUnit.Framework.DescriptionAttribute(
		"In order to interact with a Forum\r\nAs a Forum User\r\nI want to create Posts in tha" + "t Forum")]
	[NUnit.Framework.CategoryAttribute("ForumAgentSpecs")]
	public partial class ForumPostingFeature
	{
		private static ITestRunner testRunner;

#line 1 "ForumPosting.feature"
#line hidden

		[NUnit.Framework.TestFixtureSetUpAttribute()]
		public virtual void FeatureSetup()
		{
			testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
			var featureInfo = new FeatureInfo(
				new CultureInfo("en-US"),
				"Forum Posting",
				"In order to interact with a Forum\r\nAs a Forum User\r\nI want to create Posts in tha" + "t Forum",
				ProgrammingLanguage.CSharp,
				new[] { "ForumAgentSpecs" });
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

		public virtual void ScenarioSetup(ScenarioInfo scenarioInfo)
		{
			testRunner.OnScenarioStart(scenarioInfo);
		}

		public virtual void ScenarioCleanup()
		{
			testRunner.CollectScenarioErrors();
		}

		[NUnit.Framework.TestAttribute()]
		[NUnit.Framework.DescriptionAttribute("Publish Post")]
		public virtual void PublishPost()
		{
			var scenarioInfo = new ScenarioInfo("Publish Post", (string[])null);
#line 7
			this.ScenarioSetup(scenarioInfo);
#line 8
			testRunner.Given("the agent ForumAgent");
#line 10
			testRunner.When("I publish the command PublishPost");
#line 11
			testRunner.And("the command is complete");
#line 13
			testRunner.Then("the query ForumQueries returns the Post");
#line hidden
			this.ScenarioCleanup();
		}

		[NUnit.Framework.TestAttribute()]
		[NUnit.Framework.DescriptionAttribute("Publish Post in a Category")]
		public virtual void PublishPostInACategory()
		{
			var scenarioInfo = new ScenarioInfo("Publish Post in a Category", (string[])null);
#line 15
			this.ScenarioSetup(scenarioInfo);
#line 16
			testRunner.Given("the agent ForumAgent");
#line 18
			testRunner.When("I publish the command PublishPost");
#line 19
			testRunner.And("the command is complete");
#line 21
			testRunner.Then("the query CategoryQueries returns Post");
#line hidden
			this.ScenarioCleanup();
		}
	}
}

#endregion