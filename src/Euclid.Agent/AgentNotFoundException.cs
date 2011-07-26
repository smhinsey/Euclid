using System;

namespace Euclid.Agent
{
    public class AgentNotFoundException : Exception
    {
        public AgentNotFoundException(string scheme, string systemName)
            : base(string.Format("Could not find the agent {0}.{1}", scheme, systemName))
        {
        }

    }
}