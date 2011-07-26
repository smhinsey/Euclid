using System;
using System.Collections.Generic;

namespace Euclid.Framework.Cqrs.Metadata
{
    public interface IMetadata
    {
        string Name { get; set; }
        Type Type { get; set; }
        string Namespace { get; }
    }
}