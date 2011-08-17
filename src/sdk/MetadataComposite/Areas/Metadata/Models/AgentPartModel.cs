using Euclid.Framework.Agent.Metadata;

namespace MetadataComposite.Areas.Metadata.Models
{
	public class AgentPartModel : FooterLinkModel
	{
		public string AgentSystemName { get; set; }
		public string NextAction { get; set; }
		public IPartCollection Part { get; set; }
	}
}