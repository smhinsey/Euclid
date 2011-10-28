using System;
using Euclid.Framework.AgentMetadata.Extensions;

namespace Euclid.Framework.AgentMetadata
{
	public class PartMetadata : TypeMetadata, IPartMetadata
	{
		public PartMetadata(Type type)
			: base(type)
		{
		}

		public IPartCollection GetContainingPartCollection()
		{
			var agent = Type.Assembly.GetAgentMetadata();

			return agent.GetPartCollectionContainingType(Type);
		}

		public Type PartInterface
		{
			get { return GetContainingPartCollection().CollectionType; }
		}
	}
}