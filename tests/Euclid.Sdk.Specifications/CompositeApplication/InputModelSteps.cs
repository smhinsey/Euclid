using System;
using System.Linq;
using System.Threading;
using Euclid.Composites;
using Euclid.Sdk.TestAgent.Commands;
using Euclid.Sdk.TestAgent.Queries;
using Euclid.TestingSupport;
using NUnit.Framework;
using SimpleBrowser;
using TechTalk.SpecFlow;

namespace Euclid.Sdk.Specifications.CompositeApplication
{
	[Binding]
	public class InputModelSteps : DefaultSpecSteps
	{
		private Browser Browser
		{
			get
			{
                return ScenarioContext.Current["browser"] as Browser;
			}

			set
			{
				ScenarioContext.Current["browser"] = value;
			}
		}

        [Given("the TestComposite running on http://localhost:4997")]
        public void GetComposite()
        {
            Browser = new Browser();

            var composite = GetContainer().Resolve<BasicCompositeApp>();

            var agent = composite.Agents.First();

            var command = agent.Commands.Collection.Where(p => p.Name == typeof(TestCommand).Name).First();

            var url = string.Format(
                "http://localhost:4997/metadata/agents/{0}/ViewInputModelForCommand/{1}", agent.SystemName, command.Name);

            Browser.Navigate(url);

            Assert.Null(Browser.LastWebException);
        }

		[When("I fill out the input model TestInputModel")]
		public void FillOutInputModel()
		{

		    Browser.Find("Number").Value = "7";
			Browser.Find("publish-command").Click();
			
			var rawPubId = Browser.Text;
			var pubId = Guid.Parse(rawPubId);

			PubIdOfLastMessage = pubId;

            Assert.AreNotEqual(Guid.Empty, PubIdOfLastMessage);
			// if you watch the output log, you'll see that nothing happens during this sleep period. i believe that something
			// watin is doing with threading is causing problems
			// Thread.Sleep(10000);
		}

		[Then("the query TestQuery returns data")]
		public void QueryReturnsData()
		{
            var query = GetContainer().Resolve<TestQuery>();

            Assert.NotNull(query);

		    var readModels = query.FindByNumber(7);

            Assert.Greater(readModels.Count, 0);
		}
	}
}