using System;

namespace Euclid.Framework.AgentMetadata
{
	public class InterfaceMetadata : IInterfaceMetadata
	{
		public InterfaceMetadata(Type t)
		{
			if (t.IsInterface)
			{
				this.InterfaceName = t.Name;
				this.InterfaceType = t;
				this.ImplementationType = t.UnderlyingSystemType;
			}
		}

		public Type ImplementationType { get; private set; }

		public string InterfaceName { get; private set; }

		public Type InterfaceType { get; private set; }
	}
}