using System;

namespace Euclid.Framework.AgentMetadata
{
	public class MetadataFormatNotSupportedException : Exception
	{
		public MetadataFormatNotSupportedException(string format)
			: base(format)
		{
		}
	}
}