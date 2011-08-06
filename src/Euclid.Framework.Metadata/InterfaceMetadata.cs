using System;

namespace Euclid.Framework.Metadata
{
    public class InterfaceMetadata : IInterfaceMetadata
    {
        public string InterfaceName { get; private set; }
        public Type InterfaceType { get; private set; }
        public Type ImplementationType { get; private set; }

        public InterfaceMetadata(Type t)
        {
            if (t.IsInterface)
            {
                InterfaceName = t.Name;
                InterfaceType = t;
                ImplementationType = t.UnderlyingSystemType;
            }
        }
    }
}