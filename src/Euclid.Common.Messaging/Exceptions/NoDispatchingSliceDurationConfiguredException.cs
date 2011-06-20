using System;

namespace Euclid.Common.Messaging.Exceptions
{
	public class NoDispatchingSliceDurationConfiguredException : Exception
	{
		public NoDispatchingSliceDurationConfiguredException(string message) : base(message)
		{
		}
	}
}