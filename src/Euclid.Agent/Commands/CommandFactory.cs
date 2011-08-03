using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Metadata;
using Euclid.Framework.Models;

namespace Euclid.Agent.Commands
{
    public class CommandFactory
    {
        public static ICommand Get(ITypeMetadata commandMetadata, IInputModel inputModel, NameValueCollection values = null)
        {
            GuardAgentPart(commandMetadata.Type);

            var command = Activator.CreateInstance(commandMetadata.Type) as ICommand;

            if (values != null)
            {
                SetPropertyValues(commandMetadata, values, command, inputModel.Properties);
            }

            return command;
        }

        private static void SetPropertyValues(ITypeMetadata commandMetadata, NameValueCollection values, ICommand command, IEnumerable<IPropertyMetadata> properties)
        {
            foreach (var property in properties.Where(p => p.PropertyValueSetterType != null))
            {
                var setter = Activator.CreateInstance(property.PropertyValueSetterType) as IPropertyValueSetter;
                if (setter == null)
                {
                    throw new InvalidPropertySetterSpecifiedException(property.PropertyValueSetterType);
                }

                setter.SetProperty(command, commandMetadata, values);
            }
        }

        private static void GuardAgentPart(Type agentPartImplementationType)
        {
            if (!typeof(ICommand).IsAssignableFrom(agentPartImplementationType))
            {
                throw new InvalidAgentPartImplementation(agentPartImplementationType);
            }
        }
    }
}