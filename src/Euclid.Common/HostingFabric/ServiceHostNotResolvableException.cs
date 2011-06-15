using System;

namespace Euclid.Common.HostingFabric
{
	public class ServiceHostNotResolvableException : Exception
	{
		public ServiceHostNotResolvableException(string message, Exception exception) : base(message, exception)
		{
			
		}

		public ServiceHostNotResolvableException(string message) : base(message)
		{
		}
	}
}