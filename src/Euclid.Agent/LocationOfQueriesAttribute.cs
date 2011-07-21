using System;

namespace Euclid.Agent
{
	[AttributeUsage(AttributeTargets.Assembly)]
	public class LocationOfQueriesAttribute : NamespaceFinderAttribute
	{
	}
}