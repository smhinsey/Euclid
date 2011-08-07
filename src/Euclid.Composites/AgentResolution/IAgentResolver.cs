using System.Reflection;

namespace Euclid.Composites.AgentResolution
{
	public interface IAgentResolver
	{
		Assembly GetAgent(string systemName);
	}
}