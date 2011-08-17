using System;

namespace Euclid.Framework.Agent.Metadata
{
	public class PartCollectionNotFound : Exception
	{
		public PartCollectionNotFound(string partType) : base(partType)
		{
		}
	}
}