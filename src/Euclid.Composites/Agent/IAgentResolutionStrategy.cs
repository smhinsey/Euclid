using System.Reflection;

namespace Euclid.Composites.Agent
{
	public interface IAgentResolutionStrategy
	{
		Assembly GetAgent(string systemName);
	}
}