using System;

namespace Euclid.Composites
{
	public class CommandNotFoundInCompositeException : Exception
	{
		public CommandNotFoundInCompositeException(string commandName) : base(commandName)
		{
		}
	}
}