using System.Collections.Generic;
using Euclid.Framework.AgentMetadata;

namespace CompositeInspector.Models
{
	public class AgentListModel
	{
		public AgentListModel(IEnumerable<IAgentMetadata> agents)
		{
			Agents = agents;
		}

		public IEnumerable<IAgentMetadata> Agents { get; private set; }
	}
}