using System;

namespace Euclid.Composites.Conversion
{
	public class CannotCreateInputModelException : Exception
	{
		public CannotCreateInputModelException(string commandName)
			: base(string.Format("Unable to create an input model for command {0}", commandName))
		{
		}
	}
}