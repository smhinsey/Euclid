using System.Linq;
using System.Xml.Linq;
using Euclid.Framework.AgentMetadata.Extensions;
using Euclid.Framework.AgentMetadata.Formatters;
using Newtonsoft.Json;

namespace Euclid.Composites.Formatters
{
	public class CompositeMetadataFormatter : MetadataFormatter
	{
		private readonly BasicCompositeApp _compositeApp;

		public CompositeMetadataFormatter(BasicCompositeApp compositeApp)
		{
			_compositeApp = compositeApp;
		}

		protected override string GetAsXml()
		{
			var root = new XElement(
				"Composite", 
				new XAttribute("Name", _compositeApp.Name),
				new XAttribute("Description", _compositeApp.Description),
				new XAttribute("IsValid", _compositeApp.IsValid()),
				XElement.Parse(_compositeApp.Agents.GetBasicMetadataFormatter().GetRepresentation("xml")));

			var inputModels = new XElement("InputModels");
			foreach (var m in _compositeApp.InputModels)
			{
				inputModels.Add(new XElement("Model"), new XElement("Name", m.Name), new XElement("Namespace", m.Namespace));
			}

			root.Add(inputModels);

			var configurationErrors = new XElement("ConfigurationErrors");
			foreach (var s in _compositeApp.GetConfigurationErrors())
			{
				configurationErrors.Add(new XElement("Error", s));
			}

			root.Add(configurationErrors);

			return root.ToString();
		}

		public override object GetJsonObject(JsonSerializer serializer)
		{
			return
				new
					{
						_compositeApp.Name,
						_compositeApp.Description,
						IsValid = _compositeApp.IsValid(),
						Agents = _compositeApp.Agents.Select(a => new { a.DescriptiveName, a.SystemName, a.Description }),
						ConfigurationErrors = _compositeApp.GetConfigurationErrors(),
						Commands = _compositeApp.Commands.OrderBy(x=>x.Name).Select(x=> new {x.Namespace, x.Name}),
						Queries = _compositeApp.Queries.OrderBy(x => x.Name).Select(x => new { x.Namespace, x.Name, Query = x.Methods.Select(m => m.Name) }),
						InputModels = _compositeApp.InputModels.OrderBy(x=>x.Name).Select(x=>new {x.Namespace, x.Name, CommandName = _compositeApp.GetCommandMetadataForInputModel(x.Type).Name})
					};
		}
	}
}