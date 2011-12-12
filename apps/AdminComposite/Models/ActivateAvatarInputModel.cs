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

	public class ActivateCategoryInputModel : DefaultInputModel
	{
		public ActivateCategoryInputModel()
		{
			CommandType = typeof (ActivateCategory);
		}

		public Guid CategoryIdentifier { get; set; }
		public bool Active { get; set; }
	}

	public class ActivateContentInputModel : DefaultInputModel
	{
		public ActivateContentInputModel()
		{
			CommandType = typeof (ActivateContent);
		}

		public Guid ContentIdentifier { get; set; }
		public bool Active { get; set; }
	}

	public class DeleteContentInputModel : DefaultInputModel
	{
		public DeleteContentInputModel()
		{
			CommandType = typeof (DeleteForumContent);
		}

		public Guid ContentIdentifier { get; set; }
	}
}