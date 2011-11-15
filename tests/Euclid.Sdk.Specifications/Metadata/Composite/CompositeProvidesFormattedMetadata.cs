using Euclid.Sdk.TestAgent.Commands;
using Euclid.Sdk.TestComposite.Models;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Euclid.Sdk.Specifications.Metadata.Composite
{
	[Binding]
	[StepScope(Feature = "Composite provides configuration metadata")]
	public class CompositeProvidesFormattedMetadata : CompositeTestProperties
	{
		[Given("it contains the TestAgent")]
		public void RegisterTestAgent()
		{
			Assert.NotNull(Composite);

			Composite.AddAgent(typeof(TestCommand).Assembly);
		}

		[Given("it contains the TestInputModel")]
		public void RegisterTestInputModel()
		{
			Assert.NotNull(Composite);

			Composite.RegisterInputModelMap<TestInputModel, TestCommand>();
		}
	}
}