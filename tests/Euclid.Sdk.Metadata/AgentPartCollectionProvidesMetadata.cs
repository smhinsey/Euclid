using Euclid.Framework.AgentMetadata.Extensions;
using Euclid.Sdk.FakeAgent.Commands;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Euclid.Sdk.Metadata
{
	[Binding]
	public class AgentPartCollectionProvidesMetadata : PropertiesUsedInTests
	{
		[Given("a part collection (.*)")]
		public void ThePartCollection(string descriptiveName)
		{
			Agent = typeof (FakeCommand).Assembly.GetAgentMetadata();

			var partCollection = Agent.GetPartCollectionByDescriptiveName(descriptiveName);

			Assert.NotNull(partCollection);

			Formatter = partCollection.GetFormatter();
		}
	}
}