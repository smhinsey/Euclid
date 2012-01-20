using System;
using System.ComponentModel.DataAnnotations;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Areas.Forum.InputModels
{
	public class CreateTagInputModel : DefaultInputModel
	{
		public CreateTagInputModel()
		{
			CommandType = typeof(CreateTag);
		}

		public bool Active { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "You must supply the id of the user creating this category")]
		[RegularExpression(
			@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$")]
		public Guid CreatedBy { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "You must supply a forum identifier")]
		[RegularExpression(
			@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$")]
		public Guid ForumIdentifier { get; set; }

		[Required(ErrorMessage = "The tag name cannot be blank")]
		[Display(Name = "Tag Name")]
		public string Name { get; set; }
	}
}