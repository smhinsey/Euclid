using System;

namespace Euclid.Composites.Conversion
{
	public class InputModelForPartNotRegisteredException : Exception
	{
		public InputModelForPartNotRegisteredException(string partName)
			: base(string.Format("There are no input models associated with the command '{0}'", partName))
		{
		}
	}
}