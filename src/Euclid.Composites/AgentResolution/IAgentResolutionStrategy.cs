using System.Reflection;

namespace Euclid.Composites.AgentResolution
{
	public interface IAgentResolutionStrategy
	{
		Assembly GetAgent(string systemName);
	}
}