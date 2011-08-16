using System;
using System.Collections.Generic;
using System.Linq;

namespace Euclid.Framework.Agent.Metadata
{
    public class TypeMetadata : ITypeMetadata
	{
        protected TypeMetadata()
        {
            
        }

        public TypeMetadata(Type type)
		{
			Namespace = type.Namespace;
			Name = type.Name;
			Type = type;

			Interfaces = Type.GetInterfaces().Select(inf => new InterfaceMetadata(inf));
			Properties = Type.GetProperties().Select(pi => new PropertyMetadata(pi));
			Methods = Type.GetMethods().Where(mi => !mi.IsSpecialName && mi.DeclaringType != typeof(object)).Select(mi => new MethodMetadata(mi));
		}

        public IEnumerable<IInterfaceMetadata> Interfaces { get; protected set; }
        public IEnumerable<IMethodMetadata> Methods { get; protected set; }

		public string Name { get; set; }
        public string Namespace { get; protected set; }

        public IEnumerable<IPropertyMetadata> Properties { get; protected set; }

		public Type Type { get; set; }

		public IEnumerable<IPropertyMetadata> GetAttributes(Type type)
		{
			if (!typeof (IPropertyMetadata).IsAssignableFrom(type))
			{
				throw new UnexpectedTypeException(typeof (IPropertyMetadata), type);
			}

			return Type.GetCustomAttributes(type, true).Cast<IPropertyMetadata>().ToList();
		}

        public IMetadataFormatter GetFormatter()
        {
            return FormattableMetadataFactory.GetFormatter(this);
        }
	}
}