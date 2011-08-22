using System;

namespace Euclid.Framework.AgentMetadata
{
	public class AgentPartFormatterNotFoundException : Exception
	{
		public AgentPartFormatterNotFoundException(string agentPartTypeName)
			: base(agentPartTypeName)
		{
		}
	}
}