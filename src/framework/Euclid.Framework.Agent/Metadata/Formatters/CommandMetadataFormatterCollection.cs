using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Euclid.Framework.Agent.Extensions;
using Euclid.Framework.Cqrs;
using Newtonsoft.Json;

namespace Euclid.Framework.Agent.Metadata.Formatters
{
	public class CommandMetadataFormatterCollection : PartCollectionsBase<ICommand>, ICommandMetadataFormatterCollection
	{
		public CommandMetadataFormatterCollection(Assembly agent)
		{
			Initialize(agent, agent.GetCommandNamespace());
		}

        public override object GetJsonObject(JsonSerializer serializer)
        {
        }

        public override string GetAsXml()
        {
            
        }

	}
}