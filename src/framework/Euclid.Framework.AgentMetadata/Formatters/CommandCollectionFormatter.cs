using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Euclid.Framework.AgentMetadata.Formatters
{
	public class CommandCollectionFormatter : MetadataFormatter, IMetadataFormatter
	{
		private readonly IPartCollection _metadata;

		public CommandCollectionFormatter(IPartCollection metadata)
		{
			this._metadata = metadata;
		}

		protected override string GetAsXml()
		{
			var root = new XElement("Commands");

			foreach (var item in this._metadata.Collection)
			{
				root.Add(new XElement("Command", new XAttribute("Namespace", item.Namespace), new XAttribute("Name", item.Name)));
			}

			return root.ToString();
		}

		protected override object GetJsonObject(JsonSerializer serializer)
		{
			return new { Commands = this._metadata.Collection.Select(x => new { x.Namespace, x.Name, }) };
		}
	}
}