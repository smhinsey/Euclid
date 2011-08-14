using System;
using System.Collections.Generic;

namespace Euclid.Framework.Agent.Metadata
{
    public interface IMethodMetadata
    {
        string Name { get; set; }
        IEnumerable<IArgumentMetadata> Arguments { get; set; }
        Type ContainingType { get; set; }
        Type ReturnType { get; set; }
    }
}