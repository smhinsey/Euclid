using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Euclid.Framework.AgentMetadata.Extensions;
using Euclid.Framework.AgentMetadata.Formatters;
using Euclid.Framework.AgentMetadata.PartCollection;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Models;
using Newtonsoft.Json;

namespace Euclid.Framework.AgentMetadata
{
	public class AgentMetadata : IAgentMetadata
	{
		private readonly Assembly _agent;

		public AgentMetadata(Assembly agent)
		{
			_agent = agent;

			IsValid = _agent.ContainsAgent();

			if (IsValid)
			{
				DescriptiveName = _agent.GetAgentName();
				SystemName = _agent.GetAgentSystemName();

				Commands = new CommandPartCollection(_agent, _agent.GetCommandNamespace());
				Queries = new QueryPartCollection(_agent, _agent.GetQueryNamespace());
				ReadModels = new ReadModelPartCollection(_agent, _agent.GetReadModelNamespace());
			}
		}

		public Assembly AgentAssembly
		{
			get { return _agent; }
		}

		public IPartCollection Commands { get; private set; }

		public string DescriptiveName { get; private set; }

		public bool IsValid { get; private set; }

		public IPartCollection Queries { get; private set; }

		public IPartCollection ReadModels { get; private set; }

		public string SystemName { get; private set; }

		public IMetadataFormatter GetBasicMetadataFormatter()
		{
			return new BasicAgentMetadataFormatter(this);
		}

		public IMetadataFormatter GetMetadataFormatter()
		{
			return new AgentMetadataFormatter(this);
		}

		public ITypeMetadata GetPartByTypeName(string partName)
		{
			var partCollection = GetPartCollectionContainingPartName(partName);

			return partCollection.Collection.Where(m => m.Name == partName).FirstOrDefault();
		}

		public IPartCollection GetPartCollectionByDescriptiveName(string descriptiveName)
		{
			if (descriptiveName.ToLower() == "commands")
			{
				return Commands;
			}

			if (descriptiveName.ToLower() == "queries")
			{
				return Queries;
			}

			if (descriptiveName.ToLower() == "readmodels")
			{
				return ReadModels;
			}

			throw new PartCollectionNotFound(descriptiveName);
		}

		public IPartCollection GetPartCollectionContainingPartName(string partName)
		{
			var allParts = Commands.Collection.Union(ReadModels.Collection).Union(Queries.Collection);

			var part = allParts.Where(x => x.Name == partName).FirstOrDefault();

			if (part == null)
			{
				throw new PartCollectionNotFound(partName);
			}

			return GetPartCollectionContainingType(part.Type);
		}

		public IPartCollection GetPartCollectionContainingType(Type partType)
		{
			if (typeof (ICommand).IsAssignableFrom(partType))
			{
				return Commands;
			}
			else if (typeof (IQuery).IsAssignableFrom(partType))
			{
				return Queries;
			}
			else if (typeof (IReadModel).IsAssignableFrom(partType))
			{
				return ReadModels;
			}

			throw new PartCollectionNotFound(partType.Name);
		}

		private class AgentMetadataFormatter : MetadataFormatter
		{
			private readonly IAgentMetadata _agentMetadata;

			public AgentMetadataFormatter(IAgentMetadata agentMetadata)
			{
				_agentMetadata = agentMetadata;
			}

			protected override string GetAsXml()
			{
				var xml = new XElement("Agent",
				                       new XElement("DescriptiveName", _agentMetadata.DescriptiveName),
				                       new XElement("SystemName", _agentMetadata.SystemName));

				var commands = new XElement("Commands");
				foreach (var c in _agentMetadata.Commands.Collection)
				{
					commands.Add(
					             new XElement("Command",
					                          new XAttribute("Namespace", c.Namespace),
					                          new XAttribute("Name", c.Name)));
				}
				xml.Add(commands);


				var readModels = new XElement("ReadModels");
				foreach (var r in _agentMetadata.ReadModels.Collection)
				{
					readModels.Add(
					               new XElement("ReadModel",
					                            new XAttribute("Namespace", r.Namespace),
					                            new XAttribute("Name", r.Name)));
				}
				xml.Add(readModels);

				var queries = new XElement("Queries");
				foreach (var q in _agentMetadata.Queries.Collection)
				{
					queries.Add(
					            new XElement("Query",
					                         new XAttribute("Namespace", q.Namespace),
					                         new XAttribute("Name", q.Name)));
				}
				xml.Add(queries);

				return xml.ToString();
			}

			protected override object GetJsonObject(JsonSerializer serializer)
			{
				return new
				       	{
				       		_agentMetadata.DescriptiveName,
				       		_agentMetadata.SystemName,
				       		Commands = _agentMetadata.Commands.Collection.Select(x => new
				       		                                                          	{
				       		                                                          		x.Namespace,
				       		                                                          		x.Name
				       		                                                          	}),
				       		ReadModels = _agentMetadata.ReadModels.Collection.Select(x => new
				       		                                                              	{
				       		                                                              		x.Namespace,
				       		                                                              		x.Name
				       		                                                              	}),
				       		Queries = _agentMetadata.Queries.Collection.Select(x => new
				       		                                                        	{
				       		                                                        		x.Namespace,
				       		                                                        		x.Name
				       		                                                        	})
				       	};
			}
		}

		private class BasicAgentMetadataFormatter : MetadataFormatter
		{
			private readonly IAgentMetadata _agentMetadata;

			public BasicAgentMetadataFormatter(IAgentMetadata agentMetadata)
			{
				_agentMetadata = agentMetadata;
			}

			protected override string GetAsXml()
			{
				return new XElement("Agent",
				                    new XElement("DescriptiveName", _agentMetadata.DescriptiveName),
				                    new XElement("SystemName", _agentMetadata.SystemName)).ToString();
			}

			protected override object GetJsonObject(JsonSerializer serializer)
			{
				return new
				       	{
				       		_agentMetadata.DescriptiveName,
				       		_agentMetadata.SystemName
				       	};
			}
		}
	}
}