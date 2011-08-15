using System;

namespace Euclid.Framework.Agent.Metadata
{
	public class InterfaceMetadataProvider : IInterfaceMetadata
	{
		public InterfaceMetadataProvider(Type t)
		{
			if (t.IsInterface)
			{
				InterfaceName = t.Name;
				InterfaceType = t;
				ImplementationType = t.UnderlyingSystemType;
			}
		}

		public Type ImplementationType { get; private set; }

		public string InterfaceName { get; private set; }
		public Type InterfaceType { get; private set; }
	}
}