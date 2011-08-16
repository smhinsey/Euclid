using System;

namespace Euclid.Framework.Agent.Metadata
{
    public interface IInterfaceMetadata
	{
		Type ImplementationType { get; }
		string InterfaceName { get; }
		Type InterfaceType { get; }
	}
}