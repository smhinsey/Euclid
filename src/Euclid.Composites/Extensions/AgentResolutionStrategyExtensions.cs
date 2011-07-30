using System.Linq;
using System.Reflection;
using Euclid.Agent;
using Euclid.Composites.Agent;
using Euclid.Framework.Cqrs.Metadata;

namespace Euclid.Composites.Extensions
{
    public static class AgentResolutionStrategyExtensions
    {
        public static IAgentInfo GetAgentInfo(this IAgentResolutionStrategy[] resolvers, string systemName)
        {
            var agent = resolvers.Select(rslvr => rslvr.GetAgent(systemName)).FirstOrDefault(assembly => assembly != null);

            if (agent == null)
            {
                throw new AgentNotFoundException(systemName);
            }

            return agent.GetAgentInfo();
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