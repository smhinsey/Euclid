using System;

namespace Euclid.Framework.Agent.Parts
{
	public class InvalidPropertySetterSpecifiedException : Exception
	{
		public InvalidPropertySetterSpecifiedException(Type propertyValueSetterType)
		{
		}
	}
}