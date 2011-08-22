using System;

namespace Euclid.Framework.Agent
{
	public abstract class NamespaceFinderAttribute : Attribute, IAgentAttribute
	{
		private string _ns;

		private Type _type;

		public string Namespace
		{
			get
			{
				return this._ns;
			}

			set
			{
				this._ns = value;
				this.NamespaceOfType = null;
			}
		}

		public Type NamespaceOfType
		{
			get
			{
				return this._type;
			}

			set
			{
				this._type = value;

				if (value != null)
				{
					this.Namespace = this._type.Namespace;
				}
			}
		}
	}
}