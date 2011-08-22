using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Euclid.Framework.AgentMetadata.Extensions;

namespace Euclid.Framework.AgentMetadata.PartCollection
{
	public abstract class PartCollectionBase<T> : IPartCollection
		where T : IAgentPart
	{
		private readonly Assembly _agent;

		private readonly string _partNamespace;

		private string _agentSystemName;

		private IEnumerable<ITypeMetadata> _collection;

		private Type _collectionType;

		private bool _init;

		private string _ns;

		protected PartCollectionBase(Assembly agent, string partNamespace)
		{
			this._agent = agent;
			this._partNamespace = partNamespace;
		}

		public string AgentSystemName
		{
			get
			{
				if (!this._init)
				{
					this.Initialize();
				}

				return this._agentSystemName;
			}
		}

		public IEnumerable<ITypeMetadata> Collection
		{
			get
			{
				if (!this._init)
				{
					this.Initialize();
				}

				return this._collection;
			}
		}

		public Type CollectionType
		{
			get
			{
				if (!this._init)
				{
					this.Initialize();
				}

				return this._collectionType;
			}
		}

		public abstract string DescriptiveName { get; }

		public string Namespace
		{
			get
			{
				if (!this._init)
				{
					this.Initialize();
				}

				return this._ns;
			}
		}

		public IMetadataFormatter GetFormatter()
		{
			return FormattableMetadataFactory.GetFormatter(this);
		}

		protected void Initialize()
		{
			this._collection =
				this._agent.GetTypes().Where(type => type.Namespace == this._partNamespace && typeof(T).IsAssignableFrom(type)).
					Select(type => new TypeMetadata(type)).Cast<ITypeMetadata>().ToList();

			this._collectionType = typeof(T);
			this._agentSystemName = this._agent.GetAgentSystemName();
			this._ns = this._partNamespace;

			this._init = true;
		}
	}
}