using System;

namespace CompositeInspector
{
	public class CommandNotFoundInRegistryException : Exception
	{
		public CommandNotFoundInRegistryException(Guid publicationId)
		{
			throw new NotImplementedException();
		}
	}
}