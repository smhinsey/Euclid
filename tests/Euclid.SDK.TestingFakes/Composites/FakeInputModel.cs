using System;
using System.Collections.Generic;
using Euclid.Composites.Models;
using Euclid.Framework.Metadata;
using Euclid.Framework.Metadata.Extensions;

namespace Euclid.SDK.TestingFakes.Composites
{
    public class FakeInputModel : IInputModel
    {
        public string SubmittedByUser { get; set; }
        public IList<IPropertyMetadata> Properties { get; private set; }
        public Type CommandType { get; set; }
        public string AgentSystemName { get; set; }

        public FakeInputModel()
        {
            Properties = new List<IPropertyMetadata>
                             {
                                 {new PropertyMetadata("Password", typeof (string))},
                                 {new PropertyMetadata("Confirm Password", typeof (string))}
                             };

            CommandType = typeof (FakeCommand);

            AgentSystemName = this.GetType().Assembly.GetAgentInfo().SystemName;
        }
    }
}