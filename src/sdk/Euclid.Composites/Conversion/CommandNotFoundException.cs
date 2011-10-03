using System;

namespace Euclid.Composites.Conversion
{
	public class CommandNotFoundException : Exception
	{
		public CommandNotFoundException(string searchKey)
			: base(searchKey)
		{
		}
	}
}