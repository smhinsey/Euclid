using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

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

	public class ActivateBadgeInputModel : DefaultInputModel
	{
		public ActivateBadgeInputModel()
		{
			CommandType = typeof (ActivateBadge);
		}

		public Guid BadgeIdentifier { get; set; }
		public bool Active { get; set; }
	}
}