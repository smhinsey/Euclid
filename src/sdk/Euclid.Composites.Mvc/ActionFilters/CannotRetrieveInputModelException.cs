using System;

namespace Euclid.Composites.Mvc.ActionFilters
{
	public class CannotRetrieveInputModelException : Exception
	{
		public CannotRetrieveInputModelException(string inputModelName) : base(inputModelName)
		{
		}
	}
}