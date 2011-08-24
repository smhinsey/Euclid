using Euclid.Framework.AgentMetadata;

namespace CompositeControl.Areas.CompositeControl.Models
{
	public class PartModel : FooterLinkModel
	{
		public string NextActionName { get; set; }

		public ITypeMetadata TypeMetadata { get; set; }
	}
}