using System;

namespace Euclid.Framework.HostingFabric
{
	public class NoHostedServicesConfiguredException : Exception
	{
		public NoHostedServicesConfiguredException(string message)
			: base(message)
		{
		}
	}
}
