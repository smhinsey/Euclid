using System;

namespace Euclid.Composites.Conversion
{
	internal class CommandNotRegisteredException : Exception
	{
		public CommandNotRegisteredException(string commandName)
			: base(string.Format("The command {0} is not registered with this agent's CommandCollection", commandName))
		{
		}
	}
}