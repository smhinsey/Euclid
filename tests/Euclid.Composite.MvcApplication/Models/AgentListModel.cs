using System.Collections.Generic;
using Euclid.Framework.Agent.Metadata;

namespace Euclid.Composite.MvcApplication.Models
{
	public class AgentListModel
	{
		public AgentListModel(IList<IAgentMetadataFormatter> agents)
		{
			Agents = agents;
		}

		public IList<IAgentMetadataFormatter> Agents { get; private set; }
	}
}