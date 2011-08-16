using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Euclid.Framework.Agent.Extensions;
using Euclid.Framework.Agent.Metadata;
using Euclid.Framework.Models;
using Newtonsoft.Json;

namespace Euclid.Framework.Agent.Parts
{
	public class ReadModelMetadataFormatterCollection : PartCollectionsBase<IReadModel>, IReadModelMetadataFormatterCollection
	{
		public ReadModelMetadataFormatterCollection(Assembly agent)
		{
			Initialize(agent, agent.GetReadModelNamespace());
		}

		public override string GetAsXml()
		{
			var root = new XElement("ReadModels");

			foreach (var item in this)
			{
				root.Add(
				         new XElement("ReadModel",
				                      new XAttribute("Namespace", item.Namespace),
				                      new XAttribute("Name", item.Name)));
			}

			return root.ToString();
		}

		public override object GetJsonObject(JsonSerializer serializer)
		{
			return new
			       	{
			       		ReadModels = this.Select(x => new
			       		                              	{
			       		                              		x.Namespace,
			       		                              		x.Name,
			       		                              	})
			       	};
		}
	}
}