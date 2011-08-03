using System;
using System.Reflection;
using Euclid.Agent.Commands;
using Euclid.Agent.Extensions;
using Euclid.Framework.Metadata;

namespace Euclid.Agent
{
    public class AgentMetadata : IAgentMetadata
    {
        private readonly Assembly _agent;

        public AgentMetadata(Assembly agent)
        {
            _agent = agent;

            _agent = agent;
            IsValid = _agent.ContainsAgent();

            if (IsValid)
            {
                DescriptiveName = _agent.GetAgentName();
                SystemName = _agent.GetAgentSystemName();
                Commands = new CommandMetadataCollection(_agent);
            }
        }

        public string DescriptiveName { get; private set; }
        public string SystemName { get; private set; }
        public bool IsValid { get; private set; }

        public ICommandMetadataCollection Commands { get; private set; }
        public IAgentPartMetadataCollection Queries { get{throw new NotImplementedException();} }
    }
}