using System;

namespace CompositeInspector.Module
{
	public class CommandNotFoundInRegistryException : Exception
	{
		public CommandNotFoundInRegistryException(Guid publicationId)
		{
			throw new NotImplementedException();
		}
	}
}