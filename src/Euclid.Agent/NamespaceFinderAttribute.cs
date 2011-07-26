using System;

namespace Euclid.Agent
{
	public abstract class NamespaceFinderAttribute : Attribute, IAgentAttribute
	{
		private Type _type;
		private string _ns;

		public string Namespace
		{
			get { return _ns; }
			set 
			{
				_ns = value;
				NamespaceOfType = null;
			}
		}
		
		public Type NamespaceOfType
		{
			get { return _type; }
			set
			{
				_type = value;

				if (value != null)
				{
					Namespace = _type.Namespace;
				}
			}
		}
	}
}