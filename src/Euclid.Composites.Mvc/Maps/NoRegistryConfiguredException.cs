using System;

namespace Euclid.Composites.Mvc.Maps
{
	public class NoRegistryConfiguredException : Exception
	{
		public NoRegistryConfiguredException() : base("This step has no registry associated with it")
		{
		}
	}
}