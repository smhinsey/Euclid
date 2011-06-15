using System;

namespace Euclid.Common.Transport
{
	public class NoMessageProcessorsConfiguredException : Exception
	{
		public NoMessageProcessorsConfiguredException(string message) : base(message)
		{
		}
	}
}