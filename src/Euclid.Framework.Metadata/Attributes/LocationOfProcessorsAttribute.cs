using System;

namespace Euclid.Framework.Metadata.Attributes
{
	[AttributeUsage(AttributeTargets.Assembly)]
	public class LocationOfProcessorsAttribute : NamespaceFinderAttribute
	{
	}
}