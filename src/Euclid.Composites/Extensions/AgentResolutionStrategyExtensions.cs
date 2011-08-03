using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Euclid.Agent.Extensions;
using Euclid.Composites.AgentResolution;
using Euclid.Framework.Metadata;

namespace Euclid.Composites.Extensions
{
    public static class AgentResolutionStrategyExtensions
    {
        public static IAgentMetadata GetAgentMetadata(this IEnumerable<IAgentResolutionStrategy> resolvers, string systemName)
        {
            var agent = resolvers.Select(rslvr => rslvr.GetAgent(systemName)).FirstOrDefault(assembly => assembly != null);

            if (agent == null)
            {
                throw new AgentNotFoundException(systemName);
            }

            return agent.GetAgentMetadata();
        }

        public static Assembly GetAgent(this IAgentResolutionStrategy[] resolvers, string systemName)
        {
            var agent = resolvers.Select(rslvr => rslvr.GetAgent(systemName)).FirstOrDefault(assembly => assembly != null);

            if (agent == null)
            {
                throw new AgentNotFoundException(systemName);
            }

            return agent;
        }

    }
}