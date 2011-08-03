using System.Collections.Generic;
using System.Collections.Specialized;
using Euclid.Framework.Cqrs;

namespace Euclid.Framework.Metadata
{
    public interface IPropertyValueSetter
    {
        IEnumerable<string> ExpectedDestinationPropertyNames { get; }
        void SetProperty(ICommand command, ITypeMetadata commandMetadata, NameValueCollection values);
    }
}