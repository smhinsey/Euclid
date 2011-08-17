using System;

namespace Euclid.Framework.Agent.PartCollection
{
	public class InvalidPropertySetterSpecifiedException : Exception
	{
		public InvalidPropertySetterSpecifiedException(Type propertyValueSetterType)
		{
		}
	}
}