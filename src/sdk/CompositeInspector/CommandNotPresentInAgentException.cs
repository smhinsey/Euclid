using System;

namespace CompositeInspector.Module
{
	public class CommandNotPresentInAgentException : Exception
	{
		public CommandNotPresentInAgentException(string commandName) : base(commandName)
		{
		}
	}
}