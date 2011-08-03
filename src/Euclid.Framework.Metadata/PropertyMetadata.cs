using System;
using System.Reflection;

namespace Euclid.Framework.Metadata
{
    public class PropertyMetadata : IPropertyMetadata
    {
        public int Order { get; set; }
        public string Name { get; set; }
        public Type PropertyType { get; set; }
        public Type PropertyValueSetterType { get; set; }

        public PropertyMetadata()
        {
        }

        public PropertyMetadata(PropertyInfo pi)
        {
            Name = pi.Name;
            PropertyType = pi.PropertyType;
        }

        public PropertyMetadata(string name, Type type)
        {
            Name = name;
            PropertyType = type;
        }
    }
}