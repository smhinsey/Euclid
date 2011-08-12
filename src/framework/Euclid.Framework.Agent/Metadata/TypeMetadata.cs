using System;
using System.Collections.Generic;
using System.Linq;

namespace Euclid.Framework.Agent.Metadata
{
	public class TypeMetadata : ITypeMetadata
	{
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

		public IList<IInterfaceMetadata> Interfaces { get; private set; }

		public string Name { get; set; }
		public string Namespace { get; private set; }

		public IList<IPropertyMetadata> Properties { get; private set; }
		public Type Type { get; set; }

		public IList<IPropertyMetadata> GetAttributes(Type type)
		{
			if (!typeof (IPropertyMetadata).IsAssignableFrom(type))
			{
				throw new UnexpectedTypeException(typeof (IPropertyMetadata), type);
			}

			return Type.GetCustomAttributes(type, true).Cast<IPropertyMetadata>().ToList();
		}
	}
}