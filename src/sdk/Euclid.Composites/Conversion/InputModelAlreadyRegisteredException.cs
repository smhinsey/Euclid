using System;

namespace Euclid.Composites.Conversion
{
	public class InputModelAlreadyRegisteredException : Exception
	{
		public InputModelAlreadyRegisteredException(string message)
			: base(message)
		{
		}
	}
}