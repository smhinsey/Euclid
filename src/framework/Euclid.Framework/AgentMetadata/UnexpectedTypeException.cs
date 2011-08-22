using System;

namespace Euclid.Framework.AgentMetadata
{
	public class UnexpectedTypeException : Exception
	{
		private readonly Type _received;

		private Type _expected;

		public UnexpectedTypeException(Type expected, Type received)
		{
			this._expected = expected;
			this._received = received;
		}
	}
}