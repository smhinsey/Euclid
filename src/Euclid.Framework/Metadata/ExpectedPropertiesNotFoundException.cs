using System;
using System.Collections.Generic;

namespace Euclid.Framework.Metadata
{
    internal class ExpectedPropertiesNotFoundException : Exception
    {
        private readonly IEnumerable<string> _expectedDestinationPropertyNames;

        public ExpectedPropertiesNotFoundException(IEnumerable<string> expectedDestinationPropertyNames)
        {
            _expectedDestinationPropertyNames = expectedDestinationPropertyNames;
        }
    }
}