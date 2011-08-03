using System;
using System.Collections.Generic;
using System.Linq;

namespace Euclid.Framework.Metadata
{
    public class TypeMetadata : ITypeMetadata
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public string Namespace { get; private set; }

        public IList<IPropertyMetadata> Properties { get; private set; }
        public IList<IInterfaceMetadata> Interfaces { get; private set; }

        public IList<IPropertyMetadata> GetAttributes(Type type)
        {
            if (!typeof(IPropertyMetadata).IsAssignableFrom(type))
            {
                throw new InvalidAttributeTypeException(type);
            }

            return Type.GetCustomAttributes(type, true).Cast<IPropertyMetadata>().ToList();
        }

        public TypeMetadata(Type type)
        {
            Namespace = type.Namespace;
            Name = type.Name;
            Type = type;

            Interfaces = new List<IInterfaceMetadata>();
            foreach (var inf in Type.GetInterfaces())
            {
                Interfaces.Add(new InterfaceMetadata(inf));
            }

            Properties = new List<IPropertyMetadata>();
            foreach (var pi in Type.GetProperties())
            {
                Properties.Add(new PropertyMetadata(pi));
            }
        }
    }

    public class InvalidAttributeTypeException : Exception
    {
        private Type _type;

        public InvalidAttributeTypeException(Type type)
        {
            _type = type;
        }
    }
}