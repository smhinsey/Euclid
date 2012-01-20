using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Areas.Forum.InputModels
{
	public class CreateForumAvatarInputModel : DefaultInputModel
	{
		public CreateForumAvatarInputModel()
		{
			CommandType = typeof (CreateAvatar);
		}

		[Required(AllowEmptyStrings = false, ErrorMessage = "You must supply the user id of the person creating this avatar")]
		[RegularExpression(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$")]
		public Guid CreatedBy { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "You must supply a Forum Identifier")]
		[RegularExpression(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$")]
		public Guid ForumIdentifier { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "You must supply a name for this avatar")]
		[Display(Name="Name")]
		public string Name { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "You must supply a description for this avatar")]
		[Display(Name = "Decription")]
		public string Description { get; set; }
		
		[Display(Name="Image")]
		public HttpPostedFileBase Image { get; set; }
		
		public string ImageUrl { get; set; }
	}
}