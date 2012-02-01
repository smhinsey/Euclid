using System;

namespace CompositeInspector
{
	public class CommandNotFoundInAgentException : Exception
	{
		public CommandNotFoundInAgentException(string commandName) : base(commandName)
		{
		}
	}
}