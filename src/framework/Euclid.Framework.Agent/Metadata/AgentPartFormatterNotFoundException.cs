using System;

namespace Euclid.Framework.Agent.Metadata
{
    public class AgentPartFormatterNotFoundException : Exception
    {
        public AgentPartFormatterNotFoundException(string agentPartTypeName) : base(agentPartTypeName)
        {
        }
    }
}