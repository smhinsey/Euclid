using System.Reflection;
using Euclid.Framework.Metadata.Extensions;

namespace Euclid.Composites.Agent
{
	public abstract class AgentResolverBase : IAgentResolutionStrategy
	{
		public abstract Assembly GetAgent(string systemName);

		protected bool IsAgent(Assembly assembly, string systemName)
		{
			var metadata = assembly.GetAgentInfo();

			if (metadata == null) return false;

			return (systemName == metadata.SystemName);
		}
	}
}