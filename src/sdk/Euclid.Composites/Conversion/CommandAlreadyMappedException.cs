using System;

namespace Euclid.Composites.Conversion
{
	public class CommandAlreadyMappedException : Exception
	{
		public CommandAlreadyMappedException(string commandType) : base(commandType)
		{}
	}
}