using System;

namespace Euclid.Composites.Mvc.Extensions
{
	public class RequiredInputModelFieldIsEmptyException : Exception
	{
		public RequiredInputModelFieldIsEmptyException(string fieldName)
			: base(fieldName)
		{
		}
	}
}