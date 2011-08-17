using System;

namespace Euclid.Framework.Agent.Metadata
{
	public class MetadataFormatNotSupportedException : Exception
	{
		public MetadataFormatNotSupportedException(string format)
			: base(format)
		{
		}
	}
}