using System;
using System.Linq;
using System.Net.Http;
using Euclid.Framework.AgentMetadata.Extensions;
using Euclid.Sdk.TestAgent.Commands;
using Euclid.TestingSupport;
using NUnit.Framework;
using TechTalk.SpecFlow;
using WatiN.Core;

namespace Euclid.Sdk.Specifications.CompositeApplication
{
    [Binding]
    public class InputModelIsPublishedViaAgentPanel : DefaultSpecSteps, IDisposable
    {
        private IE Browser
        {
            get { return ScenarioContext.Current["browser"] as IE; }
            set { ScenarioContext.Current["browser"] = value; }
        }

        [Given("the TestComposite running on http://localhost:4997")]
        public void GetComposite()
        {
            var agent = typeof (TestCommand).Assembly.GetAgentMetadata();

            var command = agent.Commands.Collection.Where(p => p.Name == typeof (TestCommand).Name).FirstOrDefault();

            Assert.NotNull(command);

            var url = string.Format("http://localhost:4997/metadata/agents/{0}/ViewInputModelForCommand/{1}", agent.SystemName, command.Name);

            Browser = new IE(url);
        }

        [When("I fill out the input model TestInputModel")]
        public void FillOutInputModel()
        {
            Browser.TextField(Find.ByName("Number")).TypeText("7");
            Browser.Button(Find.ById("publish-command")).Click();
        }

        [Then("the command TestCommand should be marked complete")]
        public void CommandIsMarkedComplete()
        {
            ScenarioContext.Current.Pending();
        }

        [Then("the query TestQuery returns data")]
        public void QueryReturnsData()
        {
            ScenarioContext.Current.Pending();
        }

        public void Dispose()
        {
            if (Browser != null)
            {
                Browser.Dispose();
            }
        }
    }
}