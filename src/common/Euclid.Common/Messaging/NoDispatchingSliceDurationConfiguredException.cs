using System;

namespace Euclid.Common.Messaging
{
	public class NoDispatchingSliceDurationConfiguredException : Exception
	{
		public NoDispatchingSliceDurationConfiguredException(string message)
			: base(message)
		{
		}
	}
}
