using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Euclid.Framework.Agent.Metadata;
using Euclid.Framework.Agent.Parts;
using Euclid.Framework.Agent.Metadata.Formatters;

namespace Euclid.Framework.Agent
{
    public class AgentMetadata : MetadataFormatter, IAgentMetadata
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

				Commands = new CommandMetadataCollection(_agent);
				Queries = new QueryMetadataCollection(_agent);
				ReadModels = new ReadModelMetadataCollection(_agent);
			}
		}

		public Assembly AgentAssembly
		{
			get { return _agent; }
		}

		public ICommandMetadataCollection Commands { get; private set; }

		public string DescriptiveName { get; private set; }

        public string SystemName { get; private set; }

        public bool IsValid { get; private set; }
	    
	    public IQueryMetadataCollection Queries { get; private set; }
		
        public IReadModelMetadataCollection ReadModels { get; private set; }

        public string GetBasicMetadata(string format)
        {
            return new BasicMetadata(this).GetRepresentation(format);
        }

        public override object  GetJsonObject(JsonSerializer serializer)
        {
            return new {
                           DescriptiveName,
                           SystemName,
                           Commands = Commands.Select(x=>new
                                                  {
                                                      x.Namespace,
                                                      x.Name
                                                  }),
                           ReadModels = ReadModels.Select(x=>new {
                                                                 x.Namespace,
                                                                 x.Name
                                                                 }),
                           Queries = Queries.Select(x=>new {
                                                           x.Namespace,
                                                           x.Name
                                                           })
                       };
        }
    
        public override string GetAsXml()
        {
            var xml = new XElement("Agent",
                               new XElement("DescriptiveName", DescriptiveName),
                               new XElement("SystemName", SystemName));

            var commands = new XElement("Commands");
            foreach(var c in Commands)
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

        private class BasicMetadata : MetadataFormatter
        {
            private readonly IAgentMetadata _agentMetadata;

            internal BasicMetadata(IAgentMetadata agentMetadata)
            {
                _agentMetadata = agentMetadata;
            }

            public override object GetJsonObject(JsonSerializer serializer)
            {
                return new
                {
                    _agentMetadata.DescriptiveName,
                    _agentMetadata.SystemName
                };
            }

            public override string GetAsXml()
            {
                return new XElement("Agent",
                                   new XElement("DescriptiveName", _agentMetadata.DescriptiveName),
                                   new XElement("SystemName", _agentMetadata.SystemName)).ToString();

            }
        }
	}
}