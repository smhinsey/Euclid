using System;

namespace Euclid.Framework.Agent
{
	public class DuplicatePartNameException : Exception
	{
		public DuplicatePartNameException(string partTypeName, string partName) : base(string.Format("Cannot add multiple {0}s named {1} to the PartCollection", partTypeName, partName))
		{
		}
	}
}