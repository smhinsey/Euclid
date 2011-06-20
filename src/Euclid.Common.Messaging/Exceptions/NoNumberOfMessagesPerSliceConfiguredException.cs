using System;

namespace Euclid.Common.Messaging.Exceptions
{
	public class NoNumberOfMessagesPerSliceConfiguredException : Exception
	{
		public NoNumberOfMessagesPerSliceConfiguredException(string message) : base(message)
		{
		}
	}
}