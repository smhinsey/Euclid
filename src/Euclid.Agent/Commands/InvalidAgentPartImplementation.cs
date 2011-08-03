using System;

namespace Euclid.Agent.Commands
{
    public class InvalidAgentPartImplementation : Exception
    {
        private Type _type;

        public InvalidAgentPartImplementation(Type agentPartImplementationType)
        {
            _type = agentPartImplementationType;
        }
    }
}