using System;

namespace Euclid.Agent
{
	public abstract class NamespaceFinderAttribute : Attribute
	{
		public string Namespace { get; set; }
		public Type NamespaceOfType { get; set; }
	}
}