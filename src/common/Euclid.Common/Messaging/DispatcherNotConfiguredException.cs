using System;

namespace Euclid.Common.Messaging
{
	public class DispatcherNotConfiguredException : Exception
	{
		public DispatcherNotConfiguredException(string message)
			: base(message)
		{
		}

		public DispatcherNotConfiguredException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}