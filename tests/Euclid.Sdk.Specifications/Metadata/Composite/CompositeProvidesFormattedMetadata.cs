using Euclid.Common.Messaging;
using Euclid.Composites;
using Euclid.Sdk.TestAgent.Commands;
using Euclid.Sdk.TestComposite.Converters;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Euclid.Sdk.Specifications.Metadata.Composite
{
    [Binding]
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

            Composite.RegisterInputModel(new TestInputModelToCommandConverter());
        }
    }
}