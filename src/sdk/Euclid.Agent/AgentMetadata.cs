using System;
using System.Reflection;
using Euclid.Agent.Commands;
using Euclid.Agent.Extensions;
using Euclid.Framework.Agent.Metadata;

namespace Euclid.Agent
{
	public class AgentMetadata : IAgentMetadata
	{
		private readonly Assembly _agent;

		public AgentMetadata(Assembly agent)
		{
			_agent = agent;

			IsValid = _agent.ContainsAgent();

			if (IsValid)
			{
				DescriptiveName = _agent.GetAgentName();
				SystemName = _agent.GetAgentSystemName();
				Commands = new CommandMetadataCollection(_agent);
			}
		}

		public Assembly AgentAssembly
		{
			get { return _agent; }
		}

		public ICommandMetadataCollection Commands { get; private set; }

		public string DescriptiveName { get; private set; }
		public bool IsValid { get; private set; }

		public IAgentPartMetadataCollection Queries
		{
			get { throw new NotImplementedException(); }
		}

		public string SystemName { get; private set; }
	}
}