using System;

namespace Euclid.Framework.Agent.Metadata
{
    public class ArgumentMetadata : IArgumentMetadata
    {
        public string Name { get; set; }
        public Type PropertyType { get; set; }
        public int Order { get; set; }
        public object DefaultValue { get; set; }
    }
}