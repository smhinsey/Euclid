using System;

namespace Euclid.Framework.Agent
{
	public abstract class NamespaceFinderAttribute : Attribute, IAgentAttribute
	{
		private string _ns;

		private Type _type;

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
