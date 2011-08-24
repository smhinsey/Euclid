using System.Collections.Generic;
using Euclid.Framework.AgentMetadata;

namespace AgentPanel.Areas.Metadata.Models
{
	public class AgentListModel : FooterLinkModel
	{
		public AgentListModel(IEnumerable<IAgentMetadata> agents)
		{
			Agents = agents;
		}

		public IEnumerable<IAgentMetadata> Agents { get; private set; }
	}
}
