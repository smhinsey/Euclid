using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Euclid.Agent.Extensions;
using Euclid.Framework.Metadata;

namespace Euclid.Agent
{
	public abstract class PartCollectionsBase : IList<ITypeMetadata>
	{
		private IList<ITypeMetadata> _internal;
		private Type _partType;

		public ITypeMetadata this[int index]
		{
			get { return _internal[index]; }
			set { _internal[index] = value; }
		}

		public string AgentSystemName { get; private set; }

		public int Count
		{
			get { return _internal.Count; }
		}

		public bool IsReadOnly
		{
			get { return _internal.IsReadOnly; }
		}

		public string Namespace { get; private set; }

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
			throw new NotImplementedException();
		}

		public IEnumerator<ITypeMetadata> GetEnumerator()
		{
			return _internal.GetEnumerator();
		}

		public int IndexOf(ITypeMetadata item)
		{
			return _internal.IndexOf(item);
		}

		public void Insert(int index, ITypeMetadata item)
		{
			_internal.Insert(index, item);
		}

		public bool Remove(ITypeMetadata item)
		{
			throw new NotImplementedException();
		}

		public void RemoveAt(int index)
		{
			throw new NotImplementedException();
		}

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

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}