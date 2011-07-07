using System;

namespace Euclid.Common.Messaging.Exceptions
{
	public class NoInputChannelConfiguredException : Exception
	{
		public NoInputChannelConfiguredException(string message) : base(message)
		{
		}
	}
}