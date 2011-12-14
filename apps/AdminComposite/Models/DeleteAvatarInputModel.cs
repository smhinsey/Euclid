using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Models
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