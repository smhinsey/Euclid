using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Euclid.Framework.AgentMetadata.Formatters
{
	public class DefaultFormatter : MetadataFormatter
	{
		private readonly ITypeMetadata _metadata;

		public DefaultFormatter(ITypeMetadata metadata)
		{
			_metadata = metadata;
		}

		protected override string GetAsXml()
		{
			var root = new XElement(_metadata.Name, new XElement("Namespace", _metadata.Namespace));

			var props = new XElement("Properties");
			foreach (var p in _metadata.Properties)
			{
				props.Add(new XElement(p.Name, p.PropertyType.Name));
			}

			root.Add(props);

			return root.ToString();
		}

		public override object GetJsonObject(JsonSerializer serializer)
		{
			return
				new
					{
						_metadata.Name,
						_metadata.Namespace,
						Properties =
							_metadata.Properties.Select(x => new { PropertyName = x.Name, PropertyType = x.PropertyType.Name })
					};
		}
	}
}