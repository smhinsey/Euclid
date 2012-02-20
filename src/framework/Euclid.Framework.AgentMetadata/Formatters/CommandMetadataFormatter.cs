using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Euclid.Framework.AgentMetadata.Formatters
{
	internal class CommandMetadataFormatter : MetadataFormatter
	{
		private readonly ITypeMetadata _typeMetadata;

		public CommandMetadataFormatter(ITypeMetadata typeMetadata)
		{
			_typeMetadata = typeMetadata;
		}

		protected override string GetAsXml()
		{
			var root = new XElement("ReadModel");

			foreach (var p in _typeMetadata.Properties)
			{
				root.Add(
					new XElement("Property", new XElement("PropertyName", p.Name), new XElement("PropertyType", p.PropertyType.Name)));
			}

			return root.ToString();
		}

		public override object GetJsonObject(JsonSerializer serializer)
		{
			return new
			       	{
			       		Properties =
			       			_typeMetadata.Properties.Select(p => new {PropertyName = p.Name, PropertyType = p.PropertyType.Name}),
			       		_typeMetadata.Name,
			       	};
		}
	}
}