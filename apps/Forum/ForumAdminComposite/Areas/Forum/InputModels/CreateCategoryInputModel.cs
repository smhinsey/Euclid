using System;
using System.ComponentModel.DataAnnotations;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Areas.Forum.InputModels
{
	public class CreateCategoryInputModel : DefaultInputModel
	{
		public CreateCategoryInputModel()
		{
			CommandType = typeof (CreateCategory);
		}

		[Required(ErrorMessage="The category name cannot be blank")]
		[Display(Name="Category Name")]
		public string Name { get; set; }

		[Required(ErrorMessage="The category slug cannot be blank")]
		[Display(Name="Slug")]
		public string Slug { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "You must supply a forum identifier")]
		[RegularExpression(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$")]
		public Guid ForumIdentifier { get; set; }
		
		[Required(AllowEmptyStrings = false, ErrorMessage = "You must supply the id of the user creating this category")]
		[RegularExpression(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$")]
		public Guid CreatedBy { get; set; }

		public bool Active { get; set; }
	}
}