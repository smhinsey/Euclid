using System;
using System.Linq;
using System.Reflection;
using Euclid.Composites.Extensions;

namespace Euclid.Composites.Agent
{
	public class AppDomainAgentResolver : AgentResolverBase
	{
		public override Assembly GetAgent(string scheme, string systemName)
		{
			return AppDomain.CurrentDomain
				.GetAssemblies()
				.Where(
				       assembly => assembly.ContainsAgent() && IsAgent(assembly, scheme, systemName))
				.FirstOrDefault();
		}
	}
}