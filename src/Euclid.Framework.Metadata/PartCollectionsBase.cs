using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Euclid.Framework.Metadata.Extensions;

namespace Euclid.Framework.Metadata
{
    internal abstract class PartCollectionsBase : IList<ITypeMetadata>
    {
        private IList<ITypeMetadata> _internal;
        private Type _partType;
        public string Namespace { get; private set; }
        public string AgentSystemName { get; private set; }

        protected void Initialize<T>(Assembly agent, string partNamespace) where T : IAgentPart
        {
            _internal = agent.GetTypes()
                .Where(type =>
                       type.Namespace == partNamespace &&
                       typeof (T).IsAssignableFrom(type))
                .Select(type => new TypeMetadata(type) as ITypeMetadata)
                .ToList();

            _partType = typeof (T);

            AgentSystemName = agent.GetAgentSystemName();
            Namespace = partNamespace;
        }

        public IEnumerator<ITypeMetadata> GetEnumerator()
        {
            return _internal.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(ITypeMetadata item)
        {
            if (Contains(item))
            {
                throw new DuplicatePartNameException(_partType.Name, item.Name);
            }

            _internal.Add(item);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(ITypeMetadata item)
        {
            return _internal.Where(x => x.Name == item.Name).Any();
        }

        public void CopyTo(ITypeMetadata[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(ITypeMetadata item)
        {
            throw new System.NotImplementedException();
        }

        public int Count
        {
            get { return _internal.Count; }
        }

        public bool IsReadOnly
        {
            get { return _internal.IsReadOnly; }
        }

        public int IndexOf(ITypeMetadata item)
        {
            return _internal.IndexOf(item);
        }

        public void Insert(int index, ITypeMetadata item)
        {
            _internal.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            throw new System.NotImplementedException();
        }

        public ITypeMetadata this[int index]
        {
            get { return _internal[index]; }
            set { _internal[index] = value; }
        }
    }

    internal class DuplicatePartNameException : Exception
    {
        public DuplicatePartNameException(string partTypeName, string partName) : base(string.Format("Cannot add multiple {0}s named {1} to the PartCollection", partTypeName, partName))
        {}
    }
}