using System;

namespace Euclid.Agent.Parts
{
	public class InvalidPropertySetterSpecifiedException : Exception
	{
		public InvalidPropertySetterSpecifiedException(Type propertyValueSetterType)
		{
		}
	}
}