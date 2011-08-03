using System;
using System.Collections.Generic;
using Euclid.Framework.Metadata;

namespace Euclid.Framework.Models
{
    public class InputModel : IInputModel
    {
        public string SubmittedByUser { get; set; }
        public IEnumerable<IPropertyMetadata> Properties { get; private set; }
        public Type CommandType { get; set; }
        public string AgentSystemName { get; set; }

        public InputModel()
        {
            Properties = new List<IPropertyMetadata>();
        }
    }
}
