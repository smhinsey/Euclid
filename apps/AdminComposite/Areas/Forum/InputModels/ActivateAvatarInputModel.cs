using System;
using System.ComponentModel.DataAnnotations;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Areas.Forum.InputModels
{
	public class ActivateAvatarInputModel : DefaultInputModel
	{
		public ActivateAvatarInputModel()
		{
			CommandType = typeof (ActivateAvatar);
		}

		[Required(AllowEmptyStrings=false, ErrorMessage="You must supply an Avatar Identifier")]
		[RegularExpression(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$")]
		public Guid AvatarIdentifier { get; set; }

		public bool Active { get; set; }
	}
}