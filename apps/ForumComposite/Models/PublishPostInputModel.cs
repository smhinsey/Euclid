using Euclid.Composites.Mvc.Models;

namespace ForumComposite.Models
{
	public class PublishPostInputModel : InputModelBase
	{
		public string Body { get; set; }

		public string Title { get; set; }
	}
}
