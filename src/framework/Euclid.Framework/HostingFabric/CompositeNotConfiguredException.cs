using System;

namespace Euclid.Framework.HostingFabric
{
	public class CompositeNotConfiguredException : Exception
	{
		public CompositeNotConfiguredException()
			: base("Only configured composites can be installed into the hosting fabric.")
		{
		}
	}
}
