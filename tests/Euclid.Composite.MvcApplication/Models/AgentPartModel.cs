using Euclid.Framework.Agent.Metadata;

namespace Euclid.Composite.MvcApplication.Models
{
    public class AgentPartModel : FooterLinkModel
    {
        public string AgentSystemName { get; set; }
        public string NextAction { get; set; }
        public IPartCollection Part { get; set; }
    }
}