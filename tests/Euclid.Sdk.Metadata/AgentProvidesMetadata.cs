using Euclid.Framework.AgentMetadata.Extensions;
using Euclid.Sdk.FakeAgent.Commands;
using TechTalk.SpecFlow;

namespace Euclid.Sdk.Metadata
{
	[Binding]
	public class AgentProvidesMetadata : PropertiesUsedInTests
	{
		[Given("an agent")]
		public void AnAgent()
		{
			this.Agent = typeof(FakeCommand).Assembly.GetAgentMetadata();
		}

		[When("the (.*) is requested")]
		public void FormattedMetadataIsRequested(string representationType)
		{
			switch (representationType.ToLower())
			{
				case "basic":
					this.Formatter = this.Agent.GetBasicMetadataFormatter();
					break;
				case "full":
					this.Formatter = this.Agent.GetMetadataFormatter();
					break;
			}
		}
	}
}