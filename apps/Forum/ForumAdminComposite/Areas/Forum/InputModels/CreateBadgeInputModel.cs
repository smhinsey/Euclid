using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Areas.Forum.InputModels
{
	public class CreateBadgeInputModel : DefaultInputModel
	{
		public CreateBadgeInputModel()
		{
			CommandType = typeof (CreateBadge);
		}

		public enum TriggerField
		{
			NumberPosts,
			NumberComments,
			CommentsOnPost,
			PostScore,
			CommentScore
		}

		[Required(ErrorMessage = "You must supply a Forum Identifier")]
		[RegularExpression(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$")]
		public Guid ForumIdentifier { get; set; }

		[Required(ErrorMessage="You must supply a name for this badge")]
		[Display(Name="Badge Name")]
		public string Name { get; set; }

		[Required(ErrorMessage = "You must supply a description for this badge")]
		[Display(Name="Description")]
		public string Description { get; set; }

		[Required(ErrorMessage = "You must supply the field name that triggers this badge")]
		[Display(Name="Field")]
		public string Field { get; set; }

		[Required(ErrorMessage = "You must supply the operator used to evaluate whether the condition for this badge is met")]
		public string Operator { get; set; }

		[Required(ErrorMessage = "You must supply the value that triggers the badge")]
		public string Value { get; set; }

		public HttpPostedFileBase Image { get; set; }
		public string ImageUrl { get; set; }

		public SelectList Operators
		{
			get { return new SelectList(new [] {"<", "<=", "=", "<>", ">", ">="}); }
		}

		public SelectList Fields
		{
			get
			{ return new SelectList(Enum.GetNames(typeof(TriggerField)));}
		}
	}
}