using System;

namespace Euclid.Framework.Agent
{
	public abstract class LocationOfCommandsAttributeContract : Attribute
	{
		public abstract string Namespace { get; set; }
		public abstract Type NamespaceOfType { get; set; }
	}
}