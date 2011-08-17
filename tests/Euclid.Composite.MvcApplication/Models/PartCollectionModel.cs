using Euclid.Framework.Agent;
using Euclid.Framework.Agent.Metadata;

namespace Euclid.Composite.MvcApplication.Models
{
    public class PartCollectionModel : FooterLinkModel
	{
		public IPartCollection Parts { get; set; }
        public string NextActionName { get; set; }
	}
}