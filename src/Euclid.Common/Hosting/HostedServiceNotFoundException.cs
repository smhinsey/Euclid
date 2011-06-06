using System;

namespace Euclid.Common.Hosting
{
	public class HostedServiceNotFoundException : Exception
	{
		public HostedServiceNotFoundException(Guid serviceId)
			: base(string.Format("Unable to locate hosted service {0} in the service host.", serviceId))
		{
		}
	}
}