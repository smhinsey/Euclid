using System;
using System.Collections.Generic;
using Euclid.Framework.Metadata;

namespace Euclid.Composites.InputModel
{
    public interface IInputModel
    {
        string SubmittedByUser { get; set; }
        IList<IEuclidMetdata> Properties { get; }
        Type CommandType { get; set; }
        string AgentSystemName { get; set; }
    }
}