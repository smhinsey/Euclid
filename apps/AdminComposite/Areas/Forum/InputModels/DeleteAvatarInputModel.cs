using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Areas.Forum.InputModels
{
	public class DeleteAvatarInputModel : DefaultInputModel
	{
		public DeleteAvatarInputModel()
		{
			CommandType = typeof (DeleteAvatar);
		}

		public Guid AvatarIdentifier { get; set; }
	}
}