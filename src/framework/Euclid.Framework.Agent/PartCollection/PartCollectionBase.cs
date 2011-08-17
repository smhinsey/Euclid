using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Euclid.Framework.Agent.Extensions;
using Euclid.Framework.Agent.Metadata;

namespace Euclid.Framework.Agent.PartCollection
{
    public abstract class PartCollectionBase<T> : IPartCollection
        where T : IAgentPart
    {
        private readonly Assembly _agent;
        private readonly string _partNamespace;

        protected PartCollectionBase(Assembly agent, string partNamespace)
        {
            _agent = agent;
            _partNamespace = partNamespace;
        }

        private bool _init = false;
        protected void Initialize()
        {
            _collection = _agent.GetTypes()
                .Where(type =>
                       type.Namespace == _partNamespace &&
                       typeof (T).IsAssignableFrom(type))
                .Select(type => new TypeMetadata(type))
                .Cast<ITypeMetadata>()
                .ToList();

            _collectionType = typeof(T);
            _agentSystemName = _agent.GetAgentSystemName();
            _ns = _partNamespace;

            _init = true;
        }

        private string _agentSystemName;
        public string AgentSystemName
        {
            get
            {
                if (!_init) Initialize();

                return _agentSystemName;
            }

        }

        private string _ns;
        public string Namespace
        {
            get
            {
                if (!_init) Initialize();

                return _ns;
            }
        }

        public abstract string DescriptiveName { get; }

        private Type _collectionType;
        public Type CollectionType
        {
            get
            {
                if (!_init) Initialize();

                return _collectionType;
            }
        }

        private IEnumerable<ITypeMetadata> _collection;
        public IEnumerable<ITypeMetadata> Collection
        {
            get
            {
                if (!_init) Initialize();

                return _collection;
            }
        }

        public IMetadataFormatter GetFormatter()
        {
            return FormattableMetadataFactory.GetFormatter(this);
        }
    }
}
