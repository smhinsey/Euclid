using System;
using Euclid.Framework.Agent.Attributes;

namespace Euclid.Agent.Attributes
{
	[AttributeUsage(AttributeTargets.Assembly)]
	public class LocationOfQueriesAttribute : NamespaceFinderAttribute
	{
	}

	[AttributeUsage(AttributeTargets.Assembly)]
	public class LocationOfReadModelsAttribute: NamespaceFinderAttribute
	{
		
	}
}