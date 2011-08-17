using System;

namespace Euclid.Framework.Agent.Metadata
{
	public class ExpectedConstructorMissingException : Exception
	{
		public ExpectedConstructorMissingException(string typeMissingConstructor) : base(typeMissingConstructor)
		{
		}
	}
}