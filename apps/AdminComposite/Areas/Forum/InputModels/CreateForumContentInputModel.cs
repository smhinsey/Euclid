using System;
using System.ComponentModel.DataAnnotations;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Areas.Forum.InputModels
{
	public class CreateForumContentInputModel : DefaultInputModel
	{
		public CreateForumContentInputModel()
		{
			CommandType = typeof (CreateForumContent);
		}

		[Required(AllowEmptyStrings = false, ErrorMessage = "You must supply the user id of the person creating this avatar")]
		[RegularExpression(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$")]
		public Guid CreatedBy { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "You must supply a Forum Identifier")]
		[RegularExpression(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$")]
		public Guid ForumIdentifier { get; set; }

		[Required(ErrorMessage="The content location cannot be blank")]
		[Display(Name="Content Location")]
		public string Location { get; set; }

		[Required(ErrorMessage = "The content type cannot be blank")]
		[Display(Name = "Content Type")]
		public string Type { get; set; }

		[Required(ErrorMessage = "The content cannot be blank")]
		[Display(Name = "Content")]
		public string Value { get; set; }

		public bool Active { get; set; }
	}
}