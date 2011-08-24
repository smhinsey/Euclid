using System;

namespace Euclid.Framework.Agent
{
	[AttributeUsage(AttributeTargets.Assembly)]
	public class LocationOfQueriesAttribute : NamespaceFinderAttribute
	{
	}

	[AttributeUsage(AttributeTargets.Assembly)]
	public class LocationOfReadModelsAttribute : NamespaceFinderAttribute
	{
	}
}
