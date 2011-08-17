using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Euclid.Framework.Agent.Metadata.Formatters
{
    public class CommandCollectionFormatter : MetadataFormatter, IMetadataFormatter
    {
        private readonly IPartCollection _metadata;

        public CommandCollectionFormatter(IPartCollection metadata)
        {
            _metadata = metadata;
        }

        protected override object GetJsonObject(JsonSerializer serializer)
        {
            return new
                       {
                           Commands = _metadata
                                       .Collection
                                       .Select(x => new
                                                        {
                                                            x.Namespace,
                                                            x.Name,
                                                        })
                       };

        }

        protected override string GetAsXml()
        {
            var root = new XElement("Commands");

            foreach (var item in _metadata.Collection)
            {
                root.Add(
                    new XElement("Command",
                        new XAttribute("Namespace", item.Namespace),
                        new XAttribute("Name", item.Name)));
            }

            return root.ToString();
        }
    }
}