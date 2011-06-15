using System;

namespace Euclid.Common.Transport
{
	public class NoNumberOfMessagesPerSliceConfiguredException : Exception
	{
		public NoNumberOfMessagesPerSliceConfiguredException(string message) : base(message)
		{
			
		}
	}
}