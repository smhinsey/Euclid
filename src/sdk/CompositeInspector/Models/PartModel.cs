using Euclid.Framework.AgentMetadata;

namespace CompositeInspector.Models
{
	public class PartModel
	{
		public string NextActionName { get; set; }

		public ITypeMetadata TypeMetadata { get; set; }
	}
}