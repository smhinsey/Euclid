using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Euclid.Agent.Extensions;
using Euclid.Framework.Agent.Metadata;
using Euclid.Framework.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Euclid.Agent.Parts
{
	public class ReadModelMetadataCollection : PartCollectionsBase<IReadModel>, IReadModelMetadataCollection
	{
		public ReadModelMetadataCollection(Assembly agent)
		{
			Initialize(agent, agent.GetReadModelNamespace());
		}

	    public override object GetJsonObject(JsonSerializer serializer)
	    {
	        return new
	                   {
	                       ReadModels = this.Select(x => new
	                                                         {
	                                                             x.Namespace,
	                                                             x.Name,
	                                                             Properties = x.Properties.Select(
	                                                                 c => new
	                                                                          {
	                                                                              c.Name,
	                                                                              Type = c.PropertyType.Name
	                                                                          })
	                                                         })
	                   };
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
	}
}