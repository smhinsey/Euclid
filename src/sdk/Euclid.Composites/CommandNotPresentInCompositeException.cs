using System;

namespace Euclid.Composites
{
	public class CommandNotPresentInCompositeException : Exception
	{
		public CommandNotPresentInCompositeException(string commandName) : base(commandName)
		{
		}
	}
}