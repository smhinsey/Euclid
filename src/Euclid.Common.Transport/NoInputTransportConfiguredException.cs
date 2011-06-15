using System;

namespace Euclid.Common.Transport
{
	public class NoInputTransportConfiguredException : Exception
	{
		public NoInputTransportConfiguredException(string message) : base(message)
		{
		}
	}
}