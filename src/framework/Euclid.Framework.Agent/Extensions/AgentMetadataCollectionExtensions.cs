using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Euclid.Framework.Agent.Metadata;
using Euclid.Framework.Agent.Metadata.Formatters;
using Newtonsoft.Json;

namespace Euclid.Framework.Agent.Extensions
{
	public static class AgentMetadataCollectionExtensions
	{
		public static IMetadataFormatter GetBasicMetadataFormatter(this IEnumerable<IAgentMetadataFormatter> metadataList)
		{
			return new BasicAgentMetadataFormatterAggregator(metadataList);
		}

		public static IMetadataFormatter GetFullMetadataFormatter(this IEnumerable<IAgentMetadataFormatter> metadataList)
		{
			return new FullAgentMetadataFormatterAggregator(metadataList);
		}

		private class BasicAgentMetadataFormatterAggregator : MetadataFormatterFormatter
		{
			private readonly IEnumerable<IAgentMetadataFormatter> _metadataList;

			public BasicAgentMetadataFormatterAggregator(IEnumerable<IAgentMetadataFormatter> metadataList)
			{
				_metadataList = metadataList;
			}

			public override string GetAsXml()
			{
				var root = new XElement("Agents");

				foreach (var agent in _metadataList)
				{
					root.Add(XElement.Parse(agent.GetBasicMetadata("xml")));
				}

				return root.ToString();
			}

			public override object GetJsonObject(JsonSerializer serializer)
			{
				return _metadataList.Select(m => new
				                                 	{
				                                 		m.DescriptiveName,
				                                 		m.SystemName
				                                 	});
			}
		}

		private class FullAgentMetadataFormatterAggregator : MetadataFormatterFormatter
		{
			private readonly IEnumerable<IAgentMetadataFormatter> _metadataList;

			public FullAgentMetadataFormatterAggregator(IEnumerable<IAgentMetadataFormatter> metadataList)
			{
				_metadataList = metadataList;
			}

			public override string GetAsXml()
			{
				var root = new XElement("Agents");

				foreach (var agent in _metadataList)
				{
					root.Add(XElement.Parse(agent.GetRepresentation("xml")));
				}

				return root.ToString();
			}

			public override object GetJsonObject(JsonSerializer serializer)
			{
				return _metadataList.Select(m => new
				                                 	{
				                                 		m.DescriptiveName,
				                                 		m.SystemName,
				                                 		Commands = m.Commands.Select(x => new
				                                 		                                  	{
				                                 		                                  		x.Namespace,
				                                 		                                  		x.Name
				                                 		                                  	}),
				                                 		ReadModels = m.ReadModels.Select(x => new
				                                 		                                      	{
				                                 		                                      		x.Namespace,
				                                 		                                      		x.Name
				                                 		                                      	}),
				                                 		Queries = m.Queries.Select(x => new
				                                 		                                	{
				                                 		                                		x.Namespace,
				                                 		                                		x.Name
				                                 		                                	})
				                                 	});
			}
		}
	}
}