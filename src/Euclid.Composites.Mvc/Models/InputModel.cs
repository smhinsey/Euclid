using System;
using System.Collections.Generic;
using Euclid.Composites.InputModel;
using Euclid.Framework.Metadata;

namespace Euclid.Composites.Mvc.Models
{
    public class InputModel : IInputModel
    {
        public string SubmittedByUser { get; set; }
        public IList<IEuclidMetdata> Properties { get; private set; }
        public Type CommandType { get; set; }
        public string AgentSystemName { get; set; }

        public InputModel()
        {
            Properties = new List<IEuclidMetdata>();
        }
    }
}
