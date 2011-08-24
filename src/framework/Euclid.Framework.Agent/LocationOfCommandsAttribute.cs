using System;

namespace Euclid.Framework.Agent
{
	[AttributeUsage(AttributeTargets.Assembly)]
	public class LocationOfCommandsAttribute : NamespaceFinderAttribute
	{
	}
}
