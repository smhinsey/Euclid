using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace AdminComposite.Models
{
	public class ActivateAvatarInputModel : DefaultInputModel
	{
		public ActivateAvatarInputModel()
		{
			CommandType = typeof (ActivateAvatar);
		}

		public Guid AvatarIdentifier { get; set; }
		public bool Active { get; set; }
	}
}