using System;

namespace Euclid.Common.Messaging
{
	public class NoInputChannelConfiguredException : Exception
	{
		public NoInputChannelConfiguredException(string message) : base(message)
		{
		}
	}
}