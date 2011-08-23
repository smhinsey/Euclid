using Euclid.TestingSupport;
using TechTalk.SpecFlow;

namespace Euclid.Sdk.Specifications.CompositeApplication
{
    [Binding]
    public class InputModelIsPublishedViaAgentPanel : DefaultSpecSteps
    {
        [Given("a correctly configured composite")]
        public void GetComposite()
        {
        }

	    [Then("an input model")]
        public void GetInputModel()
        {
        }

	    [When("I fill out the input model")]
        public void FillOutInputModel()
        {
        }

	    [Then("the corresponding command should be marked complete")]
        public void CommandIsMarkedComplete()
        {
        }
    }
}