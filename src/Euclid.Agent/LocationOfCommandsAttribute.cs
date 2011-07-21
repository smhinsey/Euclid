using System;

namespace Euclid.Agent
{
	[AttributeUsage(AttributeTargets.Assembly)]
	public class LocationOfCommandsAttribute : NamespaceFinderAttribute
	{
	}
}