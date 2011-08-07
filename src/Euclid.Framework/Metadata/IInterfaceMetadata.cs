using System;

namespace Euclid.Framework.Metadata
{
	public interface IInterfaceMetadata
	{
		Type ImplementationType { get; }
		string InterfaceName { get; }
		Type InterfaceType { get; }
	}
}