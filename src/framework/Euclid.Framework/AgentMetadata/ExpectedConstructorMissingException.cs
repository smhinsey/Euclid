using System;

namespace Euclid.Framework.AgentMetadata
{
	public class ExpectedConstructorMissingException : Exception
	{
		public ExpectedConstructorMissingException(string typeMissingConstructor)
			: base(typeMissingConstructor)
		{
		}
	}
}
