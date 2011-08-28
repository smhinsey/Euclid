using Euclid.Framework.AgentMetadata;

namespace CompositeInspector.Models
{
	public class PartCollectionModel
	{
		public string NextActionName { get; set; }

		public IPartCollection Parts { get; set; }
	}
}