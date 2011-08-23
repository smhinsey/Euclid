using Euclid.Framework.AgentMetadata.Extensions;
using Euclid.Sdk.TestAgent.Commands;
using Euclid.Sdk.TestAgent.Queries;
using Euclid.Sdk.TestAgent.ReadModels;
using TechTalk.SpecFlow;

namespace Euclid.Sdk.Specifications.Metadata.Agent
{
	[Binding]
	public class AgentPartProvidesMetadata : PropertiesUsedInTests
	{
		[Given("the part (.*)")]
		public void ValidAgentMetadata(string partName)
		{
			Agent = typeof (TestCommand).Assembly.GetAgentMetadata();

			switch (partName.ToLower())
			{
				case "command":
                    Formatter = typeof(TestCommand).GetMetadata().GetFormatter();
					break;
				case "query":
					Formatter = typeof (TestQuery).GetMetadata().GetFormatter();
					break;
				case "readmodel":
					Formatter = typeof (TestReadModel).GetMetadata().GetFormatter();
					break;
			}
		}
	}
}