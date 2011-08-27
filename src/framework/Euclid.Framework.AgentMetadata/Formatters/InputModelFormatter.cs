using Newtonsoft.Json;

namespace Euclid.Framework.AgentMetadata.Formatters
{
    public class InputModelFormatter : MetadataFormatter, IMetadataFormatter
    {
        private readonly ITypeMetadata _inputModelMetadata;

        public InputModelFormatter(ITypeMetadata inputModelMetadata)
        {
            _inputModelMetadata = inputModelMetadata;
        }

        protected override string GetAsXml()
        {
            // jt: currently there's no good way to include the associated agent & command
            return string.Empty;
        }

        protected override object GetJsonObject(JsonSerializer serializer)
        {
            // jt: currently there's no good way to include the associated agent & command
            return null;
        }
    }
}