using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Euclid.Framework.AgentMetadata.Formatters
{
	public class QueryCollectionFormatter : MetadataFormatter
	{
		private readonly IPartCollection _metadata;

		public QueryCollectionFormatter(IPartCollection metadata)
		{
			_metadata = metadata;
		}

		protected override string GetAsXml()
		{
			var xml = new XElement("Queries");

			foreach (var item in _metadata)
			{
				xml.Add(new XElement("Query", new XElement("Namespace", item.Namespace), new XElement("Name", item.Name)));
			}

			return xml.ToString();
		}

		public override object GetJsonObject(JsonSerializer serializer)
		{
			return new { Queries = _metadata.Select(x => new { x.Namespace, x.Name }) };
		}
	}
}