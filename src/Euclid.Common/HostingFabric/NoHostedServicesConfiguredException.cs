using System;

namespace Euclid.Common.HostingFabric
{
	public class NoHostedServicesConfiguredException : Exception
	{
		public NoHostedServicesConfiguredException(string message) : base(message)
		{
			
		}
	}
}