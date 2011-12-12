using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent;
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

	public class ActivateUserInputModel : DefaultInputModel
	{
		public ActivateUserInputModel()
		{
			CommandType = typeof (ActivateForumUser);
		}

		public Guid UserIdentifier { get; set; }
		public bool Active { get; set; }
	}

	public class BlockUserInputModel : DefaultInputModel
	{
		public BlockUserInputModel()
		{
			CommandType = typeof (BlockUser);
		}

		public Guid UserIdentifier { get; set; }
	}

	public class UnblockUserInputModel : DefaultInputModel
	{
		public UnblockUserInputModel()
		{
			CommandType = typeof (UnblockUser);
		}

		public Guid UserIdentifier { get; set; }
	}

	public class UpdateForumVotingSchemeInputModel : DefaultInputModel
	{
		public UpdateForumVotingSchemeInputModel()
		{
			CommandType = typeof (UpdateForumVotingScheme);
		}

		public Guid ForumIdentifier { get; set; }
		public VotingScheme SelectedScheme { get; set; }
	}

	public class ActivateOrganizationUserInputModel : DefaultInputModel
	{
		public ActivateOrganizationUserInputModel()
		{
			CommandType = typeof (ActivateOrganizationUser);
		}

		public Guid UserIdentifier { get; set; }
		public bool Active { get; set; }
	}

	public class ApprovePostInputModel : DefaultInputModel
	{
		public ApprovePostInputModel()
		{
			CommandType = typeof (ApprovePost);
		}

		public Guid PostIdentifier { get; set; }
		public Guid ApprovedBy { get; set; }
		public Guid CreatedBy { get; set; }
	}

	public class RejectPostInputModel : DefaultInputModel
	{
		public Guid CreatedBy { get; set; }
		public Guid PostIdentifier { get; set; }
	}

	public class ApproveCommentInputModel : DefaultInputModel
	{
		public Guid PostIdentifier { get; set; }
		public Guid ApprovedBy { get; set; }
		public Guid CreatedBy { get; set; }
	}

	public class RejectCommentInputModel : DefaultInputModel
	{
		public Guid CreatedBy { get; set; }
		public Guid PostIdentifier { get; set; }
	}
}