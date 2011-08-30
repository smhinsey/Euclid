using System.Reflection;
using Euclid.Common.Logging;
using Euclid.Framework.AgentMetadata.Extensions;

namespace Euclid.Composites.AgentResolution
{
	public abstract class AgentResolverBase : DefaultLoggingSource, IAgentResolver
	{
		public abstract Assembly GetAgent(string systemName);

		protected bool IsAgent(Assembly assembly, string systemName)
		{
			var metadata = assembly.GetAgentMetadata();

			if (metadata == null)
			{
				return false;
			}

			return systemName == metadata.SystemName;
		}
	}
}