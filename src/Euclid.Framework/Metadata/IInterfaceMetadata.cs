using System;

namespace Euclid.Framework.Metadata
{
    public interface IInterfaceMetadata
    {
        string InterfaceName { get; }
        Type InterfaceType { get; }
        Type ImplementationType { get; }
    }
}