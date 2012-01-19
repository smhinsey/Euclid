using System;

namespace JsonCompositeInspector.Views.Commands
{
	public class CommandNotFoundInRegistryException : Exception
	{
		public CommandNotFoundInRegistryException(Guid publicationId) : base(publicationId.ToString())
		{
		}
	}
}

namespace JsonCompositeInspector.Module
{
	public class CommandNotFoundInRegistryException : Exception
	{
		public CommandNotFoundInRegistryException(Guid publicationId) : base(publicationId.ToString())
		{
		}
	}
}