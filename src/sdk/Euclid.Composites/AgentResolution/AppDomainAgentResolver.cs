using System;
using System.Linq;
using System.Reflection;
using Euclid.Framework.AgentMetadata.Extensions;

namespace Euclid.Composites.AgentResolution
{
	public class AppDomainAgentResolver : AgentResolverBase
	{
		public override Assembly GetAgent(string systemName)
		{
			return
				AppDomain.CurrentDomain.GetAssemblies().Where(
					assembly => assembly.ContainsAgent() && this.IsAgent(assembly, systemName)).FirstOrDefault();
		}
	}
}