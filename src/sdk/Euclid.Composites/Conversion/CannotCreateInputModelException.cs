using System;

namespace Euclid.Composites.Conversion
{
	public class CannotCreateInputModelException : Exception
	{
		public CannotCreateInputModelException(Type inputModel, string commandName)
			: base(string.Format("Unable to instantiate the input model {0} for command {1}", inputModel.FullName, commandName))
		{
		}
	}
}