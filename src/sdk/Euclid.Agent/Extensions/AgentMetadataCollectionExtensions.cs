using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Euclid.Framework.Agent.Metadata;
using Newtonsoft.Json;

namespace Euclid.Agent.Extensions
{
    public static class AgentMetadataCollectionExtensions
    {
        private class BasicAgentMetadataAggregator : MetadataFormatter
        {
            private readonly IEnumerable<IAgentMetadata> _metadataList;

            public BasicAgentMetadataAggregator(IEnumerable<IAgentMetadata> metadataList)
            {
                _metadataList = metadataList;
            }

            public override object GetJsonObject(JsonSerializer serializer)
            {
                return _metadataList.Select(m => new
                                                     {
                                                         m.DescriptiveName,
                                                         m.SystemName
                                                     });
            }

            public override string GetAsXml()
            {
                var root = new XElement("Agents");

                foreach(var agent in _metadataList)
                {
                    root.Add(XElement.Parse(agent.GetBasicMetadata("xml")));
                }

                return root.ToString();
            }
        }

        private class FullAgentMetadataAggregator : MetadataFormatter
        {
            private readonly IEnumerable<IAgentMetadata> _metadataList;

            public FullAgentMetadataAggregator(IEnumerable<IAgentMetadata> metadataList)
            {
                _metadataList = metadataList;
            }

            public override object GetJsonObject(JsonSerializer serializer)
            {
                return _metadataList.Select(m=> new {
                                                        m.DescriptiveName,
                                                        m.SystemName,
                                                        Commands = m.Commands.Select(x=>new
                                                                                            {
                                                                                                x.Namespace,
                                                                                                x.Name
                                                                                            }),
                                                        ReadModels = m.ReadModels.Select(x=>new {
                                                                                                    x.Namespace,
                                                                                                    x.Name
                                                                                                }),
                                                        Queries = m.Queries.Select(x=>new {
                                                                                              x.Namespace,
                                                                                              x.Name
                                                                                          })
                                                    });
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
        }

        public static IFormattedMetadataProvider GetBasicMetadataFormatter(this IEnumerable<IAgentMetadata> metadataList)
        {
            return new BasicAgentMetadataAggregator(metadataList);
        }

        public static IFormattedMetadataProvider GetFullMetadataFormatter(this IEnumerable<IAgentMetadata> metadataList)
        {
            return new FullAgentMetadataAggregator(metadataList);
        }
    }
}