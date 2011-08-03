using System;
using System.Reflection;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Metadata.Extensions;

namespace Euclid.Framework.Metadata
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
        public IAgentPartMetadataCollection CommandProcessors { get{throw new NotImplementedException();} }
    }
}