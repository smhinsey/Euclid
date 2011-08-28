using System;

namespace Euclid.Composites.Conversion
{
	public class PartNameNotRegisteredException : Exception
	{
		public PartNameNotRegisteredException(string partName)
			: base(string.Format("The agent part named {0} has not been registered", partName))
		{
		}
	}
}