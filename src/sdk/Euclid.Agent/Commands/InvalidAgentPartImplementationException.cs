using System;

namespace Euclid.Agent.Commands
{
	public class InvalidAgentPartImplementationException : Exception
	{
		private Type _type;

		public InvalidAgentPartImplementationException(Type agentPartImplementationType)
		{
			_type = agentPartImplementationType;
		}
	}
}