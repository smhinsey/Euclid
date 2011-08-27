using System;

namespace Euclid.Composites.Mvc.Binders
{
	public class CannotSetInputModelPropertyValues : Exception
	{
		public CannotSetInputModelPropertyValues(string inputModelTypeName, string propertyName, Exception innerException)
			: base(
				string.Format("Could not create input model {0} couldn't set Property {1}", inputModelTypeName, propertyName),
				innerException)
		{
		}
	}
}