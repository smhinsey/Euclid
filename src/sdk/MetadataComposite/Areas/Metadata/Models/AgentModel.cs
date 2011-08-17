namespace MetadataComposite.Areas.Metadata.Models
{
    public class AgentModel : FooterLinkModel
    {
        public string SystemName { get; set; }
        public string DescriptiveName { get; set; }
        public AgentPartModel Commands { get; set; }
        public AgentPartModel Queries { get; set; }
        public AgentPartModel ReadModels { get; set; }
    }
}