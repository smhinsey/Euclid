using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Euclid.Composites.Mvc.Models;
using ForumAgent;
using ForumAgent.Commands;

namespace AdminComposite.Areas.Forum.InputModels
{
	public class CreateForumInputModel : DefaultInputModel
	{
		public CreateForumInputModel()
		{
			CommandType = typeof(CreateForum);
		}

		[Required(ErrorMessage = "You must supply the user id of the person creating this avatar")]
		[RegularExpression(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$")]
		public Guid CreatedBy { get; set; }

		[Display(Name="Description")]
		public string Description { get; set; }

		[Required(ErrorMessage="The forum name cannot be blank")]
		[Display(Name="Forum Name")]
		public string Name { get; set; }

		[Required(ErrorMessage = "You must supply the user id of the person creating this avatar")]
		[RegularExpression(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$")]
		public Guid OrganizationId { get; set; }

		[Required(ErrorMessage = "Host name cannot be blank")]
		[Display(Name="Host Name")]
		public string UrlHostName { get; set; }

		[Required(ErrorMessage="The slug cannot be blank")]
		[Display(Name="Slug")]
		public string UrlSlug { get; set; }

		[Required(ErrorMessage="A voting scheme must be specified")]
		public VotingScheme VotingScheme { get; set; }

		[Required(ErrorMessage="A theme must be specified")]
		[DefaultValue("Swiss")]
		public string Theme { get; set; }

		public bool Moderated { get; set; }

		public bool Private { get; set; }

		public IList<string> AvailableHosts { get; set; }
	}
}