using System;

namespace Euclid.Framework.Agent
{
	[AttributeUsage(AttributeTargets.Assembly)]
	public class LocationOfReadModelsAttribute : LocationOfReadModelsAttributeContract
		{
			private string _ns;
			private Type _type;

			public override string Namespace
			{
				get { return _ns; }
				set
				{
					_ns = value;
					NamespaceOfType = null;
				}
			}

			public override Type NamespaceOfType
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