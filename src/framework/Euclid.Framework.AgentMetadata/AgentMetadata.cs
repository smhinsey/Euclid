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
			this._agent = agent;

			this.IsValid = this._agent.ContainsAgent();

			if (this.IsValid)
			{
				this.DescriptiveName = this._agent.GetAgentName();
				this.SystemName = this._agent.GetAgentSystemName();

				this.Commands = new CommandPartCollection(this._agent, this._agent.GetCommandNamespace());
				this.Queries = new QueryPartCollection(this._agent, this._agent.GetQueryNamespace());
				this.ReadModels = new ReadModelPartCollection(this._agent, this._agent.GetReadModelNamespace());
			}
		}

		public Assembly AgentAssembly
		{
			get
			{
				return this._agent;
			}
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
			var partCollection = this.GetPartCollectionContainingPartName(partName);

			return partCollection.Collection.Where(m => m.Name == partName).FirstOrDefault();
		}

		public IPartCollection GetPartCollectionByDescriptiveName(string descriptiveName)
		{
			if (descriptiveName.ToLower() == "commands")
			{
				return this.Commands;
			}

			if (descriptiveName.ToLower() == "queries")
			{
				return this.Queries;
			}

			if (descriptiveName.ToLower() == "readmodels")
			{
				return this.ReadModels;
			}

			throw new PartCollectionNotFound(descriptiveName);
		}

		public IPartCollection GetPartCollectionContainingPartName(string partName)
		{
			var allParts = this.Commands.Collection.Union(this.ReadModels.Collection).Union(this.Queries.Collection);

			var part = allParts.Where(x => x.Name == partName).FirstOrDefault();

			if (part == null)
			{
				throw new PartCollectionNotFound(partName);
			}

			return this.GetPartCollectionContainingType(part.Type);
		}

		public IPartCollection GetPartCollectionContainingType(Type partType)
		{
			if (typeof(ICommand).IsAssignableFrom(partType))
			{
				return this.Commands;
			}
			else if (typeof(IQuery).IsAssignableFrom(partType))
			{
				return this.Queries;
			}
			else if (typeof(IReadModel).IsAssignableFrom(partType))
			{
				return this.ReadModels;
			}

			throw new PartCollectionNotFound(partType.Name);
		}

		private class AgentMetadataFormatter : MetadataFormatter
		{
			private readonly IAgentMetadata _agentMetadata;

			public AgentMetadataFormatter(IAgentMetadata agentMetadata)
			{
				this._agentMetadata = agentMetadata;
			}

			protected override string GetAsXml()
			{
				var xml = new XElement(
					"Agent", 
					new XElement("DescriptiveName", this._agentMetadata.DescriptiveName), 
					new XElement("SystemName", this._agentMetadata.SystemName));

				var commands = new XElement("Commands");
				foreach (var c in this._agentMetadata.Commands.Collection)
				{
					commands.Add(new XElement("Command", new XAttribute("Namespace", c.Namespace), new XAttribute("Name", c.Name)));
				}

				xml.Add(commands);

				var readModels = new XElement("ReadModels");
				foreach (var r in this._agentMetadata.ReadModels.Collection)
				{
					readModels.Add(new XElement("ReadModel", new XAttribute("Namespace", r.Namespace), new XAttribute("Name", r.Name)));
				}

				xml.Add(readModels);

				var queries = new XElement("Queries");
				foreach (var q in this._agentMetadata.Queries.Collection)
				{
					queries.Add(new XElement("Query", new XAttribute("Namespace", q.Namespace), new XAttribute("Name", q.Name)));
				}

				xml.Add(queries);

				return xml.ToString();
			}

			protected override object GetJsonObject(JsonSerializer serializer)
			{
				return
					new
						{
							this._agentMetadata.DescriptiveName, 
							this._agentMetadata.SystemName, 
							Commands = this._agentMetadata.Commands.Collection.Select(x => new { x.Namespace, x.Name }), 
							ReadModels = this._agentMetadata.ReadModels.Collection.Select(x => new { x.Namespace, x.Name }), 
							Queries = this._agentMetadata.Queries.Collection.Select(x => new { x.Namespace, x.Name })
						};
			}
		}

		private class BasicAgentMetadataFormatter : MetadataFormatter
		{
			private readonly IAgentMetadata _agentMetadata;

			public BasicAgentMetadataFormatter(IAgentMetadata agentMetadata)
			{
				this._agentMetadata = agentMetadata;
			}

			protected override string GetAsXml()
			{
				return
					new XElement(
						"Agent", 
						new XElement("DescriptiveName", this._agentMetadata.DescriptiveName), 
						new XElement("SystemName", this._agentMetadata.SystemName)).ToString();
			}

			protected override object GetJsonObject(JsonSerializer serializer)
			{
				return new { this._agentMetadata.DescriptiveName, this._agentMetadata.SystemName };
			}
		}
	}
}