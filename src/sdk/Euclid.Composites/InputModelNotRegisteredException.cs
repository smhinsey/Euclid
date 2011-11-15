using System;

namespace Euclid.Composites
{
	public class InputModelNotRegisteredException : Exception
	{
		public InputModelNotRegisteredException(Type type) : base(type.FullName)
		{
		}
	}
}