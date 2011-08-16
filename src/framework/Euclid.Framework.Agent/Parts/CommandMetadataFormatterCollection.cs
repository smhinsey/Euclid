using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Euclid.Framework.Agent.Extensions;
using Euclid.Framework.Agent.Metadata;
using Euclid.Framework.Cqrs;
using Newtonsoft.Json;

namespace Euclid.Framework.Agent.Parts
{
	public class CommandMetadataFormatterCollection : PartCollectionsBase<ICommand>, ICommandMetadataFormatterCollection
	{
		public CommandMetadataFormatterCollection(Assembly agent)
		{
			Initialize(agent, agent.GetCommandNamespace());
		}

		public override string GetAsXml()
		{
			var root = new XElement("Commands");

			foreach (var item in this)
			{
				root.Add(
				         new XElement("Command",
				                      new XAttribute("Namespace", item.Namespace),
				                      new XAttribute("Name", item.Name)));
			}

			return root.ToString();
		}

		public override object GetJsonObject(JsonSerializer serializer)
		{
			return new
			       	{
			       		Commands = this.Select(x => new
			       		                            	{
			       		                            		x.Namespace,
			       		                            		x.Name,
			       		                            	})
			       	};
		}
	}
}