using System;

namespace Euclid.Common.Messaging.Exceptions
{
	public class NoInputTransportConfiguredException : Exception
	{
		public NoInputTransportConfiguredException(string message) : base(message)
		{
		}
	}
}