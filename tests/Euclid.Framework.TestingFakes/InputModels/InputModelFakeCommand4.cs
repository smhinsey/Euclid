using System;
using System.Collections.Generic;
using System.Linq;
using Euclid.Framework.Metadata;
using Euclid.Framework.Models;
using Euclid.Framework.TestingFakes.Cqrs;

namespace Euclid.Framework.TestingFakes.InputModels
{
    public class InputModelFakeCommand4 : IInputModel
    {
        public InputModelFakeCommand4()
        {
            Properties = new List<IPropertyMetadata>()
                             {
                                 new PropertyMetadata
                                     {
                                         Name = "Password",
                                         PropertyType = typeof (string),
                                         Order = 0,
                                         PropertyValueSetterType = null
                                     },

                                 new PropertyMetadata
                                     {
                                         Name = "Confirm Password",
                                         PropertyType = typeof (string),
                                         Order = 1,
                                         PropertyValueSetterType = typeof(HashedPasswordSetter)
                                     },

                                 new PropertyMetadata
                                     {
                                         Name = "Your Birthday",
                                         PropertyType = typeof (DateTime),
                                         Order = 2,
                                         PropertyValueSetterType = null
                                     }
                             };

            CommandType = typeof (FakeCommand4);
        }

        public string AgentSystemName { get; set; }
        public Type CommandType { get; private set; }
        private IEnumerable<IPropertyMetadata> _properties;
        public IEnumerable<IPropertyMetadata> Properties
        {
            get { return _properties.OrderBy(x=>x.Order).Select(x=>x); }
            private set { _properties = value; }
        }
    }
}