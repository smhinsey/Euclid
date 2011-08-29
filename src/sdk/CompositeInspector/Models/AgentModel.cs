namespace CompositeInspector.Models
{
	public class AgentModel
	{
		public string AgentSystemName { get; set; }

		public AgentPartModel Commands { get; set; }

		public string Description { get; set; }

		public string DescriptiveName { get; set; }

		public AgentPartModel Queries { get; set; }

		public AgentPartModel ReadModels { get; set; }
	}
}