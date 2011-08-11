using System;

namespace Euclid.Framework.HostingFabric
{
	public class CompositeAlreadyInstalledException : Exception
	{
		public CompositeAlreadyInstalledException()
			: base("A composite has already been installed")
		{
		}
	}
}