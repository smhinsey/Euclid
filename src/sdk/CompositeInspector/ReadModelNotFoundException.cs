using System;

namespace CompositeInspector
{
	public class ReadModelNotFoundExceptin : Exception
	{
		public ReadModelNotFoundExceptin(string readModelName) : base(readModelName)
		{
		}
	}
}