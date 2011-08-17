using System;

namespace Euclid.Framework.Agent.PartCollection
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