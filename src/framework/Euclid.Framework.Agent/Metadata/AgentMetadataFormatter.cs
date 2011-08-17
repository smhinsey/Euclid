
d:\Projects\Euclid\platform>@git.exe %*
﻿using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Euclid.Framework.Agent.Extensions;
using Euclid.Framework.Agent.Metadata.Formatters;
using Euclid.Framework.Agent.Parts;
using Newtonsoft.Json;

namespace Euclid.Framework.Agent.Metadata
{
	public class AgentMetadataFormatter : MetadataFormatterFormatter, IAgentMetadataFormatter
	{
		private readonly Assembly _agent;

		public AgentMetadataFormatter(Assembly agent)
		{
			_agent = agent;

			IsValid = _agent.ContainsAgent();

			if (IsValid)
			{
				DescriptiveName = _agent.GetAgentName();
				SystemName = _agent.GetAgentSystemName();

				Commands = new CommandMetadataFormatterCollection(_agent);
				Queries = new QueryMetadataFormatterCollection(_agent);
				ReadModels = new ReadModelMetadataFormatterCollection(_agent);
			}
		}

		public Assembly AgentAssembly
		{
			get { return _agent; }
		}

		public ICommandMetadataFormatterCollection Commands { get; private set; }

		public string DescriptiveName { get; private set; }

		public bool IsValid { get; private set; }

		public IQueryMetadataFormatterCollection Queries { get; private set; }

		public IReadModelMetadataFormatterCollection ReadModels { get; private set; }
		public string SystemName { get; private set; }

		public override string GetAsXml()
		{
			var xml = new XElement("Agent",
			                       new XElement("DescriptiveName", DescriptiveName),
			                       new XElement("SystemName", SystemName));

			var commands = new XElement("Commands");
			foreach (var c in Commands)
			{
				commands.Add(
				             new XElement("Command",
				                          new XAttribute("Namespace", c.Namespace),
				                          new XAttribute("Name", c.Name)));
			}
			xml.Add(commands);


			var readModels = new XElement("ReadModels");
			foreach (var r in ReadModels)
			{
				readModels.Add(
				               new XElement("ReadModel",
				                            new XAttribute("Namespace", r.Namespace),
				                            new XAttribute("Name", r.Name)));
			}
			xml.Add(readModels);

			var queries = new XElement("Queries");
			foreach (var q in Queries)
			{
				queries.Add(
				            new XElement("Query",
				                         new XAttribute("Namespace", q.Namespace),
				                         new XAttribute("Name", q.Name)));
			}
			xml.Add(queries);

			return xml.ToString();
		}

		public override object GetJsonObject(JsonSerializer serializer)
		{
			return new
			       	{
			       		DescriptiveName,
			       		SystemName,
			       		Commands = Commands.Select(x => new
			       		                                	{
			       		                                		x.Namespace,
			       		                                		x.Name
			       		                                	}),
			       		ReadModels = ReadModels.Select(x => new
			       		                                    	{
			       		                                    		x.Namespace,
			       		                                    		x.Name
			       		                                    	}),
			       		Queries = Queries.Select(x => new
			       		                              	{
			       		                              		x.Namespace,
			       		                              		x.Name
			       		                              	})
			       	};
		}

		public string GetBasicMetadata(string format)
		{
			return new BasicMetadataFormatter(this).GetRepresentation(format);
		}

		private class BasicMetadataFormatter : MetadataFormatterFormatter
		{
			private readonly IAgentMetadataFormatter _agentMetadataFormatter;

			internal BasicMetadataFormatter(IAgentMetadataFormatter agentMetadataFormatter)
			{
				_agentMetadataFormatter = agentMetadataFormatter;
			}

			public override string GetAsXml()
			{
				return new XElement("Agent",
				                    new XElement("DescriptiveName", _agentMetadataFormatter.DescriptiveName),
				                    new XElement("SystemName", _agentMetadataFormatter.SystemName)).ToString();
			}

			public override object GetJsonObject(JsonSerializer serializer)
			{
				return new
				       	{
				       		_agentMetadataFormatter.DescriptiveName,
				       		_agentMetadataFormatter.SystemName
				       	};
			}
		}
	}
}
d:\Projects\Euclid\platform>@set ErrorLevel=%ErrorLevel%

d:\Projects\Euclid\platform>@rem Restore the original console codepage.

d:\Projects\Euclid\platform>@chcp %cp_oem% > nul < nul
