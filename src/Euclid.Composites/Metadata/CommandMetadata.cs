using System;
using System.Collections.Generic;
using System.Linq;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Cqrs.Metadata;

namespace Euclid.Composites.Metadata
{
    public class CommandMetadata : ICommandMetadata
    {
        public IList<IPropertyMetadata> Properties { get; private set; }
        public IList<IMetadata> Interfaces { get; private set; }

        public CommandMetadata(Type commandType)
        {
            Namespace = commandType.Namespace;
            Name = commandType.Name;
            Type = commandType;

            Interfaces = new List<IMetadata>();
            foreach (var inf in Type.GetInterfaces())
            {
                Interfaces.Add(Metadata.Extract(inf));
            }

            Properties = new List<IPropertyMetadata>();
            foreach (var pi in Type.GetProperties())
            {
                Properties.Add(PropertyMetadata.Extract(pi));
            }
        }


        public string Name { get; set; }
        public Type Type { get; set; }
        public string Namespace { get; private set; }
    }
}