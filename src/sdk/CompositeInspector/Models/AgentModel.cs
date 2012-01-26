using Euclid.Framework.AgentMetadata;

namespace CompositeInspector.Models
{
	public class AgentModel
	{
		public string AgentSystemName { get; set; }

		public IPartCollection Commands { get; set; }

		public string Description { get; set; }

		public string DescriptiveName { get; set; }

		public IPartCollection Queries { get; set; }

		public IPartCollection ReadModels { get; set; }
	}
}