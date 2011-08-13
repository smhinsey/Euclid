using System;

namespace Euclid.Framework.Agent.Metadata
{
	public class UnexpectedTypeException : Exception
	{
		private readonly Type _received;
		private Type _expected;

		public UnexpectedTypeException(Type expected, Type received)
		{
			_expected = expected;
			_received = received;
		}
	}
}