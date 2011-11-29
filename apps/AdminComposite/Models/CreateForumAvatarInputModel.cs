using System;
using System.Web;
using Euclid.Composites.Mvc.Models;

namespace AdminComposite.Models
{
	public class CreateForumAvatarInputModel : DefaultInputModel
	{
		public Guid CreatedBy { get; set; }
		public Guid ForumIdentifier { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public HttpPostedFileBase Image { get; set; }
	}
}