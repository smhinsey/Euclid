using System;

namespace Euclid.Composites
{
	public class CommandNotPresentInAgentException : Exception
	{
		public CommandNotPresentInAgentException(string commandName) : base(commandName)
		{
		}
	}
}