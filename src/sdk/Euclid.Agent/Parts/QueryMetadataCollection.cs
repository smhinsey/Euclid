using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Euclid.Agent.Extensions;
using Euclid.Framework.Agent.Metadata;
using Euclid.Framework.Cqrs;
using Newtonsoft.Json;

namespace Euclid.Agent.Parts
{
	public class QueryMetadataCollection : PartCollectionsBase<IQuery>, IQueryMetadataCollection
	{
		public QueryMetadataCollection(Assembly agent)
		{
			Initialize(agent, agent.GetQueryNamespace());
		}

	    public override object GetJsonObject(JsonSerializer serializer)
	    {
	        return new
	                   {
	                       Queries = this.Select(x => new
	                                            {
                                                    x.Namespace,
                                                    x.Name,
	                                                Query = x.Methods.Select(
	                                                    q => new
	                                                             {
	                                                                 Name = q.Name,
	                                                                 Arguments = q.Arguments
                                                                                    .OrderBy(arg=>arg.Order)
                                                                                    .Select(a => new
	                                                                                        {
	                                                                                            ParameterName = a.Name,
	                                                                                            ParameterType = a.PropertyType.Name
	                                                                                        }),
                                                                    //jt: the provider excpects queries to return an ICollection<IAgentPart>
                                                                    Returns = GetFormattedReturnType(q)
	                                                             })
	                                            })
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
                var queryPart = new XElement("QueryPart",
                                new XElement("Namespace", item.Namespace),
                                new XElement("Name", item.Name));

                foreach (var method in item.Methods)
                {
                    var query = new XElement("Query",
                                                new XElement("Returns", new XCData(GetFormattedReturnType(method))),
                                                new XElement("Name", method.Name));

                        
                    foreach (var argument in method.Arguments.OrderBy(a=>a.Order))
                    {
                        query.Add(
                            new XElement("ParameterName", argument.Name),
                            new XElement("ParameterType", argument.PropertyType.Name));
                    }

                    queryPart.Add(query);
                }

                xml.Add(queryPart);
            }

	        return xml.ToString();
	    }
	}
}