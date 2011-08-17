using System.Collections.Generic;
using Euclid.Framework.Agent.Metadata;

namespace Euclid.Composite.MvcApplication.Models
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