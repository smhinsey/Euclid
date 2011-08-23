namespace AgentPanel.Areas.Metadata.Models
{
	public class AgentModel : FooterLinkModel
	{
		public AgentPartModel Commands { get; set; }

		public string DescriptiveName { get; set; }

		public AgentPartModel Queries { get; set; }

		public AgentPartModel ReadModels { get; set; }

		public string SystemName { get; set; }
	}
}