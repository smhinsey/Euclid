using System;

namespace CompositeInspector.Module
{
	public class ReadModelNotFoundExceptin : Exception
	{
		public ReadModelNotFoundExceptin(string readModelName) : base(readModelName)
		{
		}
	}
}