using System;

namespace Euclid.Agent
{
	[AttributeUsage(AttributeTargets.Assembly)]
	public class LocationOfProcessorsAttribute : NamespaceFinderAttribute
	{
	}
}