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
		"In order to interact with a Forum\r\nAs a Forum User\r\nI want to create and maintain" + " a Profile")]
	[NUnit.Framework.CategoryAttribute("ForumAgentSpecs")]
	public partial class UserProfilesFeature
	{
		private static ITestRunner testRunner;

#line 1 "UserProfile.feature"
#line hidden

		[NUnit.Framework.TestFixtureSetUpAttribute()]
		public virtual void FeatureSetup()
		{
			testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
			var featureInfo = new FeatureInfo(
				new CultureInfo("en-US"),
				"User Profiles",
				"In order to interact with a Forum\r\nAs a Forum User\r\nI want to create and maintain" + " a Profile",
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
		[NUnit.Framework.DescriptionAttribute("Register a Profile")]
		public virtual void RegisterAProfile()
		{
			var scenarioInfo = new ScenarioInfo("Register a Profile", (string[])null);
#line 7
			this.ScenarioSetup(scenarioInfo);
#line 8
			testRunner.Given("the agent ForumAgent");
#line 10
			testRunner.When("I publish the command RegisterUser");
#line 11
			testRunner.And("the command is complete");
#line 13
			testRunner.Then("the query UserQueries returns the Profile");
#line hidden
			this.ScenarioCleanup();
		}

		[NUnit.Framework.TestAttribute()]
		[NUnit.Framework.DescriptionAttribute("Update a Profile")]
		public virtual void UpdateAProfile()
		{
			var scenarioInfo = new ScenarioInfo("Update a Profile", (string[])null);
#line 16
			this.ScenarioSetup(scenarioInfo);
#line 17
			testRunner.Given("the agent ForumAgent");
#line 19
			testRunner.When("I publish the command RegisterUser");
#line 20
			testRunner.And("the command is complete");
#line 22
			testRunner.When("I publish the command UpdateUserProfile");
#line 23
			testRunner.And("the command is complete");
#line 25
			testRunner.Then("the query UserQueries returns the updated Profile");
#line hidden
			this.ScenarioCleanup();
		}

		[NUnit.Framework.TestAttribute()]
		[NUnit.Framework.DescriptionAttribute("Authenticate as User")]
		public virtual void AuthenticateAsUser()
		{
			var scenarioInfo = new ScenarioInfo("Authenticate as User", (string[])null);
#line 27
			this.ScenarioSetup(scenarioInfo);
#line 28
			testRunner.Given("the agent ForumAgent");
#line 30
			testRunner.When("I publish the command RegisterUser");
#line 31
			testRunner.And("the command is complete");
#line 33
			testRunner.Then("the query UserQueries can authenticate");
#line hidden
			this.ScenarioCleanup();
		}
	}
}

#endregion