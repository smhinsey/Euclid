using System;

namespace Euclid.Common.HostingFabric
{
	public class NoServiceHostConfiguredException : Exception
	{
		public NoServiceHostConfiguredException(string message) : base(message)
		{
		}
	}
}