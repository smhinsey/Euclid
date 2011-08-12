using System;
using System.Linq;
using System.Reflection;
using Euclid.Agent.Extensions;

namespace Euclid.Composites.AgentResolution
{
	public class AppDomainAgentResolver : AgentResolverBase
	{
		public override Assembly GetAgent(string systemName)
		{
			return AppDomain.CurrentDomain
				.GetAssemblies()
				.Where(
				       assembly => assembly.ContainsAgent() && IsAgent(assembly, systemName))
				.FirstOrDefault();
		}
	}
}