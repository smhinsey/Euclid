using Euclid.Framework.AgentMetadata;
using TechTalk.SpecFlow;

namespace Euclid.Sdk.Specifications.Metadata
{
	public class PropertiesUsedInTests
	{
		protected IAgentMetadata Agent
		{
			get { return ScenarioContext.Current["Agent"] as IAgentMetadata; }

			set { ScenarioContext.Current["Agent"] = value; }
		}

		protected string Format
		{
			get { return ScenarioContext.Current["Format"].ToString(); }

			set { ScenarioContext.Current["Format"] = value; }
		}

		protected IMetadataFormatter Formatter
		{
			get { return ScenarioContext.Current["Formatter"] as IMetadataFormatter; }

			set { ScenarioContext.Current["Formatter"] = value; }
		}
	}
}
