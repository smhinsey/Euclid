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
        private class BasicAgentMetadataFormatterAggregator : MetadataFormatter
        {
            private readonly IEnumerable<IAgentMetadata> _metadataList;

            public BasicAgentMetadataFormatterAggregator(IEnumerable<IAgentMetadata> metadataList)
            {
                _metadataList = metadataList;
            }

            protected override object GetJsonObject(JsonSerializer serializer)
            {
                return _metadataList.Select(m => new
                                                     {
                                                         m.DescriptiveName,
                                                         m.SystemName
                                                     });
            }

            protected override string GetAsXml()
            {
                var root = new XElement("Agents");

                foreach(var agent in _metadataList)
                {
                    root.Add(XElement.Parse(agent.GetBasicRepresentation("xml")));
                }

                return root.ToString();
            }
        }

        private class FullAgentMetadataFormatterAggregator : MetadataFormatter
        {
            private readonly IEnumerable<IAgentMetadata> _metadataList;

            public FullAgentMetadataFormatterAggregator(IEnumerable<IAgentMetadata> metadataList)
            {
                _metadataList = metadataList;
            }

            protected override object GetJsonObject(JsonSerializer serializer)
            {
                return _metadataList.Select(m=> new {
                                                        m.DescriptiveName,
                                                        m.SystemName,
                                                        Commands = m.Commands.Collection.Select(x=>new
                                                                                            {
                                                                                                x.Namespace,
                                                                                                x.Name
                                                                                            }),
                                                        ReadModels = m.ReadModels.Collection.Select(x => new
                                                        {
                                                                                                    x.Namespace,
                                                                                                    x.Name
                                                                                                }),
                                                        Queries = m.Queries.Collection.Select(x => new
                                                        {
                                                                                              x.Namespace,
                                                                                              x.Name
                                                                                          })
                                                    });
            }

            protected override string GetAsXml()
            {
                var root = new XElement("Agents");

                foreach (var agent in _metadataList)
                {
                    root.Add(XElement.Parse(agent.GetRepresentation("xml")));
                }

                return root.ToString();
            }
        }

        public static IMetadataFormatter GetBasicMetadataFormatter(this IEnumerable<IAgentMetadata> metadataList)
        {
            return new BasicAgentMetadataFormatterAggregator(metadataList);
        }

        public static IMetadataFormatter GetFullMetadataFormatter(this IEnumerable<IAgentMetadata> metadataList)
        {
            return new FullAgentMetadataFormatterAggregator(metadataList);
        }
    }
}