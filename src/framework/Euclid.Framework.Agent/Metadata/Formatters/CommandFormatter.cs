using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Euclid.Framework.Agent.Metadata.Formatters
{
    internal class CommandFormatter : MetadataFormatter
    {
        private readonly ITypeMetadata _typeMetadata;

        public CommandFormatter(ITypeMetadata typeMetadata)
        {
            _typeMetadata = typeMetadata;
        }

        protected override object GetJsonObject(JsonSerializer serializer)
        {
            return _typeMetadata.Properties.Select(p => new
            {
                PropertyName = p.Name,
                PropertyType = p.PropertyType.Name
            });
        }

        protected override string GetAsXml()
        {
            var root = new XElement("ReadModel");

            foreach (var p in _typeMetadata.Properties)
            {
                root.Add(
                    new XElement("Property",
                                 new XElement("PropertyName", p.Name),
                                 new XElement("PropertyType", p.PropertyType.Name)));
            }

            return root.ToString();
        }
    }
}