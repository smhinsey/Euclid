using System;

namespace Euclid.Composites.Conversion
{
	public class CommandAlreadyMappedException : Exception
	{
		private readonly Type _commandType;

		public CommandAlreadyMappedException(Type commandType)
		{
			_commandType = commandType;
		}
	}
}