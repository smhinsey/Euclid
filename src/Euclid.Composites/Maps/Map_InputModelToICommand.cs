using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Euclid.Framework.Metadata;
using Euclid.Framework.Cqrs;

namespace Euclid.Composites.Maps
{
	public class CommandFactory
	{
        public static ICommand Create(ITypeMetadata commandMetadata, NameValueCollection values = null)
        {
            if (commandMetadata == null)
            {
                throw new ArgumentNullException("commandMetadata");
            }

            var command = Activator.CreateInstance(commandMetadata.Type) as ICommand;

            var properties = commandMetadata.GetAttributes(typeof (InputProperty));

            if (values != null)
            {
                SetPropertyValues(commandMetadata, values, command, properties);
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
	}

    public class InvalidPropertySetterSpecifiedException : Exception
    {
        public InvalidPropertySetterSpecifiedException(Type propertyValueSetterType)
        {
            
        }
    }
}
