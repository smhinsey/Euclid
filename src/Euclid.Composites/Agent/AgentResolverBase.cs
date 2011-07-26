using System.Reflection;
using Euclid.Composites.Extensions;

namespace Euclid.Composites.Agent
{
	public abstract class AgentResolverBase : IAgentResolutionStrategy
	{
		public abstract Assembly GetAgent(string scheme, string systemName);

		protected bool IsAgent(Assembly assembly, string scheme, string systemName)
		{
			var metadata = assembly.GetAgentMetadata();

			if (metadata == null) return false;

			return (scheme == metadata.Scheme && systemName == metadata.SystemName);
		}
	}
}