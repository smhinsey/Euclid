using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Euclid.Framework.Cqrs;

namespace Euclid.Framework.Metadata
{
    public class HashedPasswordSetter : IPropertyValueSetter
    {
        public IEnumerable<string> ExpectedDestinationPropertyNames { get; private set; }

        public HashedPasswordSetter()
        {
            ExpectedDestinationPropertyNames = new List<string> {"PasswordSalt", "PasswordHash"};
        }

        public void SetProperty(ICommand command, ITypeMetadata commandMetadata, NameValueCollection values)
        {
            GuardExpectedProperties(commandMetadata);

            // jt: are these magic strings ok?

            var password = values["Properties.Password"];

            SetValue(command, "PasswordSalt", "salted: " + password);

            SetValue(command, "PasswordHash", "hased: " + password);
        }

        private void GuardExpectedProperties(ITypeMetadata metadata)
        {
            if (metadata.Properties.Select(p => p.Name).Intersect(ExpectedDestinationPropertyNames).Count() != ExpectedDestinationPropertyNames.Count())
            {
                throw new ExpectedPropertiesNotFoundException(ExpectedDestinationPropertyNames);
            }
        }

        private void SetValue(IAgentPart part, string destinationPropertyName, object value)
        {
            var property = part.GetType().GetProperties().Where(pi => pi.Name == destinationPropertyName).First();

            property.SetValue(part, value, null);
        }
    }
}