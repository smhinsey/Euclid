using System;

namespace Euclid.Framework.HostingFabric
{
	public class NoServiceHostConfiguredException : Exception
	{
		public NoServiceHostConfiguredException(string message) : base(message)
		{
		}
	}
}