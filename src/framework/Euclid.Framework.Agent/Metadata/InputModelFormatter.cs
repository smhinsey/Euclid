using System;
using Newtonsoft.Json;

namespace Euclid.Framework.Agent.Metadata
{
    internal class InputModelFormatter : MetadataFormatter
    {
        private readonly TypeMetadata _typeMetadata;

        public InputModelFormatter(TypeMetadata typeMetadata)
        {
            _typeMetadata = typeMetadata;
        }

        public override object GetJsonObject(JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override string GetAsXml()
        {
            throw new NotImplementedException();
        }
    }
}