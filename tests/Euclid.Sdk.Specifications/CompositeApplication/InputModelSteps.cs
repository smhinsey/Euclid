using System;
using System.Linq;
using System.Threading;
using Euclid.Composites;
using Euclid.Sdk.TestAgent.Commands;
using Euclid.TestingSupport;
using TechTalk.SpecFlow;
using WatiN.Core;

namespace Euclid.Sdk.Specifications.CompositeApplication
{
	[Binding]
	public class InputModelSteps : DefaultSpecSteps, IDisposable
	{
		private IE Browser
		{
			get
			{
				return ScenarioContext.Current["browser"] as IE;
			}

			set
			{
				ScenarioContext.Current["browser"] = value;
			}
		}

		public void Dispose()
		{
			if (Browser != null)
			{
				Browser.Dispose();
			}
		}

		[When("I fill out the input model TestInputModel")]
		public void FillOutInputModel()
		{
			Browser.TextField(Find.ByName("Number")).TypeText("7");
			Browser.Button(Find.ById("publish-command")).Click();
			
			var rawPubId = Browser.Text;
			var pubId = Guid.Parse(rawPubId);

			PubIdOfLastMessage = pubId;

			// if you watch the output log, you'll see that nothing happens during this sleep period. i believe that something
			// watin is doing with threading is causing problems
			Thread.Sleep(10000);
		}

		[Given("the TestComposite running on http://localhost:4997")]
		public void GetComposite()
		{
			var composite = GetContainer().Resolve<BasicCompositeApp>();

			var agent = composite.Agents.First();

			var command = agent.Commands.Collection.Where(p => p.Name == typeof(TestCommand).Name).First();

			var url = string.Format(
				"http://localhost:4997/metadata/agents/{0}/ViewInputModelForCommand/{1}", agent.SystemName, command.Name);

			Browser = new IE(url);
		}

		[Then("the query TestQuery returns data")]
		public void QueryReturnsData()
		{
			ScenarioContext.Current.Pending();
		}
	}
}