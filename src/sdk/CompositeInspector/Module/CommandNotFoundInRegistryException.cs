using System;

// TODO: this is odd...

namespace CompositeInspector.Views.Commands
{
	public class CommandNotFoundInRegistryException : Exception
	{
		public CommandNotFoundInRegistryException(Guid publicationId)
			: base(publicationId.ToString())
		{
		}
	}
}

namespace CompositeInspector.Module
{
	public class CommandNotFoundInRegistryException : Exception
	{
		public CommandNotFoundInRegistryException(Guid publicationId)
			: base(publicationId.ToString())
		{
		}
	}
}