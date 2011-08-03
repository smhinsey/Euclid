using System;
using System.Collections.Generic;
using Euclid.Framework.Metadata;

namespace Euclid.Framework.Models
{
    public interface IInputModel
    {
        string AgentSystemName { get; set; }
        Type CommandType { get; }
        IEnumerable<IPropertyMetadata> Properties { get; }
    }
}