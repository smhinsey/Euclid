using System.Collections.Generic;
using Euclid.Framework.AgentMetadata;

namespace MetadataComposite.Areas.Metadata.Models
{
	public class AgentListModel : FooterLinkModel
	{
		public AgentListModel(IList<IAgentMetadata> agents)
		{
			this.Agents = agents;
		}

		public IList<IAgentMetadata> Agents { get; private set; }
	}
}