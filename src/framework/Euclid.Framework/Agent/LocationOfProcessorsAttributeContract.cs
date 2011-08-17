using System;

namespace Euclid.Framework.Agent
{
	public abstract class LocationOfProcessorsAttributeContract : Attribute
	{
		public abstract string Namespace { get; set; }
		public abstract Type NamespaceOfType { get; set; }
	}
}