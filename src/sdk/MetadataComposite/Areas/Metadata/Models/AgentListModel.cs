using System.Collections.Generic;
using Euclid.Framework.AgentMetadata;

namespace AgentPanel.Areas.Metadata.Models
{
	public class AgentListModel : FooterLinkModel
	{
		public AgentListModel(IList<IAgentMetadata> agents)
		{
			Agents = agents;
		}

		public IList<IAgentMetadata> Agents { get; private set; }
	}
}