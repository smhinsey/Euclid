using System;
using System.Web;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Areas.Forum.InputModels
{
	public class UpdateForumAvatarInputModel : DefaultInputModel
	{
		public UpdateForumAvatarInputModel()
		{
			CommandType = typeof (UpdateAvatar);
		}

		public Guid AvatarIdentifier { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public HttpPostedFileBase Image { get; set; }
		public string ImageUrl { get; set; }
	}
}