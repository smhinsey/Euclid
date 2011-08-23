using System;

namespace Euclid.Framework.AgentMetadata
{
	public class PartCollectionNotFound : Exception
	{
		public PartCollectionNotFound(string partType)
			: base(partType)
		{
		}
	}
}