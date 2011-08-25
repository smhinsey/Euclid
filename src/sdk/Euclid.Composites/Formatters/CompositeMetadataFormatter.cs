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
			var root = new XElement("Composite", 
			                        XElement.Parse(_compositeApp.Agents.GetBasicMetadataFormatter().GetRepresentation("xml")));

			var inputModels = new XElement("InputModels");
			foreach (var m in _compositeApp.InputModels)
			{
				inputModels.Add(new XElement("Model"), 
				                new XElement("Name", m.Name), 
				                new XElement("Namespace", m.Namespace));
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

		protected override object GetJsonObject(JsonSerializer serializer)
		{
			return new
			       	{
			       		Agents = _compositeApp.Agents.Select(a => new
			       		                                          	{
			       		                                          		a.DescriptiveName, 
			       		                                          		a.SystemName
			       		                                          	}), 
			       		InputModels = _compositeApp.InputModels.Select(im => new
			       		                                                     	{
			       		                                                     		im.Name, 
			       		                                                     		im.Namespace
			       		                                                     	}), 
			       		ConfigurationErrors = _compositeApp.GetConfigurationErrors()
			       	};
		}
	}
}
