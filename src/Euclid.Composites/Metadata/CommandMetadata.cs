using System;
using System.Collections.Generic;
using System.Linq;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Cqrs.Metadata;

namespace Euclid.Composites.Metadata
{
    public class CommandMetadata
        : Metadata, ICommandMetadata
    {
        public IList<IPropertyMetadata> Properties { get; private set; }

        public CommandMetadata(Type commandType) : base(commandType)
        {
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
    }
}