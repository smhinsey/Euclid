using System;
using System.Collections.Generic;
using Euclid.Framework.Cqrs.Metadata;

namespace Euclid.Composites.Metadata
{
    public class CommandMetadata : ICommandMetadata
    {
        public IList<IPropertyMetadata> Properties { get; private set; }
        public IList<IMetadata> Interfaces { get; private set; }
        public string AgentSystemName { get; set; }

        public CommandMetadata(Type commandType)
        {
            Namespace = commandType.Namespace;
            Name = commandType.Name;
            CommandType = commandType;

            Interfaces = new List<IMetadata>();
            foreach (var inf in CommandType.GetInterfaces())
            {
                Interfaces.Add(Metadata.Extract(inf));
            }

            Properties = new List<IPropertyMetadata>();
            foreach (var pi in CommandType.GetProperties())
            {
                Properties.Add(PropertyMetadata.Extract(pi));
            }
        }


        public string Name { get; set; }
        public Type CommandType { get; set; }
        public string Namespace { get; private set; }
    }
}