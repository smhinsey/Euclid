using System;

namespace Euclid.Common.HostingFabric
{
	public class HostedServiceNotResolvableException : Exception
	{
		public HostedServiceNotResolvableException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}