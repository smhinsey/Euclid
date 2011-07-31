using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Euclid.Framework.Metadata.Extensions;

namespace Euclid.Framework.Metadata
{
    public class EuclidMetadata : IEuclidMetdata
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public string Namespace { get; private set; }

        public IList<PropertyInfo> Properties { get; private set; }
        public IList<Type> Interfaces { get; private set; }
        public IList<Type> Attributes { get; private set; }

        public EuclidMetadata(Type type)
        {
            Namespace = type.Namespace;
            Name = type.Name;
            Type = type;

            Interfaces = new List<Type>();
            foreach (var inf in Type.GetInterfaces())
            {
                Interfaces.Add(inf.UnderlyingSystemType);
            }

            Properties = new List<PropertyInfo>();
            foreach (var pi in Type.GetProperties())
            {
                Properties.Add(pi);
            }

            Attributes = new List<Type>();
            foreach (var attr in Type.GetCustomAttributes(true))
            {
                Attributes.Add(attr.GetType());
            }
        }
    }
}