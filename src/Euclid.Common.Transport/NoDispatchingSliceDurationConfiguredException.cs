using System;

namespace Euclid.Common.Transport
{
	public class NoDispatchingSliceDurationConfiguredException : Exception
	{
		public NoDispatchingSliceDurationConfiguredException(string message) : base(message)
		{
			
		}
	}
}