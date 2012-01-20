using System;
using System.ComponentModel.DataAnnotations;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Areas.Forum.InputModels
{
	public class CreateStopWordInputModel : DefaultInputModel
	{
		public CreateStopWordInputModel()
		{
			CommandType = typeof(CreateStopWord);
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

		[Required(ErrorMessage = "The word to match cannot be blank")]
		[Display(Name = "Word to Match")]
		public string WordToMatch { get; set; }

		[Required(ErrorMessage = "The word to replace cannot be blank")]
		[Display(Name = "Replacement Word")]
		public string ReplacementWord { get; set; }
	}
}