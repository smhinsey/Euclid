using System;

namespace Euclid.Composites.Mvc.Binders
{
	public class PartCollectionNotFoundException : Exception
	{
		public PartCollectionNotFoundException(string partType)
			: base(string.Format("Agents have no metadata collection named '{0}'", partType))
		{
		}

		public PartCollectionNotFoundException()
		{
		}
	}
}