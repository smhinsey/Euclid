using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Euclid.Composites.AgentResolution;
using Euclid.Framework.AgentMetadata;
using Euclid.Framework.AgentMetadata.Extensions;

namespace Euclid.Composites.Extensions
{
	public static class AgentResolversExtensions
	{
		public static Assembly GetAgent(this IEnumerable<IAgentResolver> resolvers, string systemName)
		{
			var agent = resolvers.Select(rslvr => rslvr.GetAgent(systemName)).FirstOrDefault(assembly => assembly != null);

			if (agent == null)
			{
				throw new AgentNotFoundException(systemName);
			}

			return agent;
		}

		public static IAgentMetadata GetAgentMetadata(this IEnumerable<IAgentResolver> resolvers, string systemName)
		{
			var agent = resolvers.Select(rslvr => rslvr.GetAgent(systemName)).FirstOrDefault(assembly => assembly != null);

			if (agent == null)
			{
				throw new AgentNotFoundException(systemName);
			}

			return agent.GetAgentMetadata();
		}
	}
}