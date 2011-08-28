using System;

namespace Euclid.Composites
{
	public class InvalidConfigurationException : Exception
	{
		public InvalidConfigurationException(string message)
			: base(message)
		{
		}
	}
}