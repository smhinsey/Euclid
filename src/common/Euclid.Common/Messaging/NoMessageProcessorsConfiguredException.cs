using System;

namespace Euclid.Common.Messaging
{
	public class NoMessageProcessorsConfiguredException : Exception
	{
		public NoMessageProcessorsConfiguredException(string message) : base(message)
		{
		}
	}
}