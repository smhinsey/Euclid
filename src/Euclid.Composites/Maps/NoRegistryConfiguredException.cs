using System;

namespace Euclid.Composites.Maps
{
	public class NoRegistryConfiguredException : Exception
	{
		public NoRegistryConfiguredException() : base("This step has no registry associated with it")
		{
		}
	}
}