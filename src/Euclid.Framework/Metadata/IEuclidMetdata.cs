using System;
using System.Collections.Generic;
using System.Reflection;

namespace Euclid.Framework.Metadata
{
    public interface IEuclidMetdata
    {
        Type Type { get; set; }
        string Name { get; set; }
        string Namespace { get; }

        IList<PropertyInfo> Properties { get; }
        IList<Type> Interfaces { get; }
        IList<Type> Attributes { get; }
    }
}