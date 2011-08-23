using Euclid.Framework.AgentMetadata.Extensions;
using Euclid.Sdk.FakeAgent.Commands;
using Euclid.Sdk.FakeAgent.Queries;
using Euclid.Sdk.FakeAgent.ReadModels;
using TechTalk.SpecFlow;

namespace Euclid.Sdk.Metadata
{
	[Binding]
	public class AgentPartProvidesMetadata : PropertiesUsedInTests
	{
		[Given("the part (.*)")]
		public void ValidAgentMetadata(string partName)
		{
			Agent = typeof(FakeCommand).Assembly.GetAgentMetadata();

			switch (partName.ToLower())
			{
				case "command":
					Formatter = typeof(FakeCommand).GetMetadata().GetFormatter();
					break;
				case "query":
					Formatter = typeof(FakeQuery).GetMetadata().GetFormatter();
					break;
				case "readmodel":
					Formatter = typeof(FakeReadModel).GetMetadata().GetFormatter();
					break;
			}
		}
	}
}