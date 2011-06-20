using System;

namespace Euclid.Common.Messaging.Exceptions
{
	public class NoMessageProcessorsConfiguredException : Exception
	{
		public NoMessageProcessorsConfiguredException(string message) : base(message)
		{
		}
	}
}