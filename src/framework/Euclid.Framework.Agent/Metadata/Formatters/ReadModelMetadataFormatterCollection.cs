using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Euclid.Framework.Agent.Extensions;
using Euclid.Framework.Models;
using Newtonsoft.Json;

namespace Euclid.Framework.Agent.Metadata.Formatters
{
	public class ReadModelMetadataFormatterCollection : PartCollectionsBase<IReadModel>, IReadModelMetadataFormatterCollection
	{
		public ReadModelMetadataFormatterCollection(Assembly agent)
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
	                                                         })
	                   };
	    }

        public override string GetAsXml()
        {
        }
	}
}