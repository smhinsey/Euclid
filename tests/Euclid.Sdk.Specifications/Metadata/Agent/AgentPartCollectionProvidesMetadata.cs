using Euclid.Framework.AgentMetadata.Extensions;
using Euclid.Sdk.TestAgent.Commands;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Euclid.Sdk.Specifications.Metadata.Agent
{
	[Binding]
	public class AgentPartCollectionProvidesMetadata : PropertiesUsedInTests
	{
		[Given("a part collection (.*)")]
		public void ThePartCollection(string descriptiveName)
		{
			Agent = typeof (TestCommand).Assembly.GetAgentMetadata();

			var partCollection = Agent.GetPartCollectionByDescriptiveName(descriptiveName);

			Assert.NotNull(partCollection);

			Formatter = partCollection.GetFormatter();
		}
	}
}
