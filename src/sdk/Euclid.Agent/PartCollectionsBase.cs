using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Euclid.Agent.Parts;
using Euclid.Framework.Agent.Extensions;
using Euclid.Framework.Agent.Metadata;
using Euclid.Framework.Agent.Metadata.Formatters;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Models;

namespace Euclid.Agent
{
    public abstract class PartCollectionsBase<TAgentPart> : MetadataFormatter, IList<IFormattableMetadata>
		where TAgentPart : IAgentPart
	{
		private List<IFormattableMetadata> _internal;
		private Type _partType;

        public IFormattableMetadata this[int index]
		{
			get { return _internal[index]; }
			set { _internal[index] = FormattableMetadata.Factory(value.Type); }
		}

		public string AgentSystemName { get; private set; }

		public int Count
		{
			get { return _internal.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public string Namespace { get; private set; }

        public void Add(IFormattableMetadata item)
		{
			if (Contains(item))
			{
				throw new DuplicatePartNameException(_partType.Name, item.Name);
			}

			_internal.Add(new FormattableMetadata(item));
		}

		public void Clear()
		{
			throw new NotImplementedException();
		}

        public bool Contains(IFormattableMetadata item)
		{
			return _internal.Where(x => x.Name == item.Name).Any();
		}

        public void CopyTo(IFormattableMetadata[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

        public IEnumerator<IFormattableMetadata> GetEnumerator()
		{
			return _internal.GetEnumerator();
		}

        public IFormattableMetadata GetMetadata<TImplementationType>() where TImplementationType : IAgentPart
		{
			return GetMetadata(typeof (TImplementationType));
		}

        public IFormattableMetadata GetMetadata(Type agentPartImplementationType)
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

        public IFormattableMetadata GetMetadata(string agentPartImplementationName)
		{
			var partImplementationType = this.Where(m => m.Name == agentPartImplementationName).Select(m => m.Type).FirstOrDefault();

			if (partImplementationType == null)
			{
				throw new PartNotRegisteredException(agentPartImplementationName, Namespace);
			}

			return GetMetadata(partImplementationType);
		}

        public int IndexOf(IFormattableMetadata item)
		{
			return _internal.IndexOf(item);
		}

        public void Insert(int index, IFormattableMetadata item)
		{
			_internal.Insert(index, item);
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

        public bool Remove(IFormattableMetadata item)
		{
			throw new NotImplementedException();
		}

		public void RemoveAt(int index)
		{
			throw new NotImplementedException();
		}

		protected void Initialize(Assembly agent, string partNamespace)
		{
		    var partTypes = agent.GetTypes()
				                .Where(type =>
				                       type.Namespace == partNamespace &&
				                       typeof (TAgentPart).IsAssignableFrom(type))
				                .Select(type => type)
				                .ToList();

		    _internal = partTypes
                            .Select(FormattableMetadata.Factory)
		                    .ToList();

			_partType = typeof (TAgentPart);
			AgentSystemName = agent.GetAgentSystemName();
			Namespace = partNamespace;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private void guardAgentPart(Type agentPartImplementationType)
		{
			if (agentPartImplementationType == null)
			{
				throw new ArgumentNullException("agentPartImplementationType");
			}

			if (!typeof (TAgentPart).IsAssignableFrom(agentPartImplementationType))
			{
				throw new InvalidAgentPartImplementationException(agentPartImplementationType);
			}
		}
	}
}