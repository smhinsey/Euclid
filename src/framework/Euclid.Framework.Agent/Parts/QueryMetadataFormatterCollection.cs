using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Euclid.Framework.Agent.Extensions;
using Euclid.Framework.Agent.Metadata;
using Euclid.Framework.Cqrs;
using Newtonsoft.Json;

namespace Euclid.Framework.Agent.Parts
{
	public class QueryMetadataFormatterCollection : PartCollectionsBase<IQuery>, IQueryMetadataFormatterCollection
	{
		public QueryMetadataFormatterCollection(Assembly agent)
		{
			Initialize(agent, agent.GetQueryNamespace());
		}

	    public override object GetJsonObject(JsonSerializer serializer)
	    {
	        return new
	                   {
	                       Queries = this.Select(
                                        x => new
                                                 {
                                                     x.Namespace,
                                                     x.Name
                                                 }
                                )
	                   };
	    }

	    private static string GetFormattedReturnType(IMethodMetadata methodMetadata)
	    {
	        return (methodMetadata.ReturnType.Namespace != null && !methodMetadata.ReturnType.Namespace.Contains("Collection"))
	                   ? methodMetadata.ReturnType.Name
	                   : string.Format("List<{0}>", methodMetadata.ReturnType.GetGenericArguments()[0].Name);
	    }

	    public override string GetAsXml()
	    {
	        var xml = new XElement("Queries");

            foreach(var item in this)
            {
                xml.Add(new XElement("Query",
                                     new XElement("Namespace", item.Namespace),
                                     new XElement("Name", item.Name)));
            }

	        return xml.ToString();
	    }
	}
}