using System;

namespace Euclid.Framework.Agent
{
	public abstract class LocationOfQueriesAttributeContract : Attribute
	{
		public abstract string Namespace { get; set; }
		public abstract Type NamespaceOfType { get; set; }
	}
}