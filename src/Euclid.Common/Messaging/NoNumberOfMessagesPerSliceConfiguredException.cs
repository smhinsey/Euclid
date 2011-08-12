using System;

namespace Euclid.Common.Messaging
{
	public class NoNumberOfMessagesPerSliceConfiguredException : Exception
	{
		public NoNumberOfMessagesPerSliceConfiguredException(string message) : base(message)
		{
		}
	}
}