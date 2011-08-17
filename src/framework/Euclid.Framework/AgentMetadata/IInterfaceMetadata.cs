using System;

namespace Euclid.Framework.AgentMetadata
{
	public interface IInterfaceMetadata
	{
		Type ImplementationType { get; }
		string InterfaceName { get; }
		Type InterfaceType { get; }
	}
}