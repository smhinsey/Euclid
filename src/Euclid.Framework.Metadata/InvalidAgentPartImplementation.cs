using System;

namespace Euclid.Framework.Metadata
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