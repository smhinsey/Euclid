using System;
using System.Linq;
using System.Reflection;
using Euclid.Agent.Extensions;
using Euclid.Framework.Agent.Metadata;
using Euclid.Framework.Cqrs;

namespace Euclid.Agent.Commands
{
	public class CommandMetadataCollection : PartCollectionsBase, ICommandMetadataCollection
	{
		public CommandMetadataCollection(Assembly agent)
		{
			Initialize<ICommand>(agent, agent.GetCommandNamespace());
		}

		public ITypeMetadata GetMetadata<TImplementationType>() where TImplementationType : IAgentPart
		{
			return GetMetadata(typeof (TImplementationType));
		}

		public ITypeMetadata GetMetadata(Type agentPartImplementationType)
		{
			var metadata = this.Where(x =>
			                          x.Namespace == agentPartImplementationType.Namespace &&
			                          x.Name == agentPartImplementationType.Name).FirstOrDefault();

			if (metadata == null)
			{
				throw new PartNotRegisteredException(agentPartImplementationType);
			}

			return metadata;
		}

		public ITypeMetadata GetMetadata(string agentPartImplementationName)
		{
			var partImplementationType = this.Where(m => m.Name == agentPartImplementationName).Select(m => m.Type).FirstOrDefault();

			if (partImplementationType == null)
			{
				throw new PartNotRegisteredException(agentPartImplementationName, Namespace);
			}

			return GetMetadata(partImplementationType);
		}

		public bool Registered(string agentPartImplementationName)
		{
			return this.Where(p => p.Name == agentPartImplementationName).Any();
		}

		public bool Registered<TImplementationType>()
		{
			return Registered(typeof (TImplementationType));
		}

		public bool Registered(Type agentPartImplementationType)
		{
			guardAgentPart(agentPartImplementationType);

			return this.Where(x =>
			                  x.Namespace == agentPartImplementationType.Namespace &&
			                  x.Name == agentPartImplementationType.Name)
				.Any();
		}

		private void guardAgentPart(Type agentPartImplementationType)
		{
			if (!typeof (ICommand).IsAssignableFrom(agentPartImplementationType))
			{
				throw new InvalidAgentPartImplementationException(agentPartImplementationType);
			}
		}
	}
}