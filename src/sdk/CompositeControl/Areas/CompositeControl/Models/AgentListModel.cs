using System.Collections.Generic;
using Euclid.Framework.AgentMetadata;

namespace CompositeControl.Areas.CompositeControl.Models
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