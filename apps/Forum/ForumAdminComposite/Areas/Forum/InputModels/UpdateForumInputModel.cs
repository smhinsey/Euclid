using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Areas.Forum.InputModels
{
	public class UpdateForumInputModel : DefaultInputModel
	{
		public UpdateForumInputModel()
		{
			CommandType = typeof(UpdateForum);
		}

		public Guid ForumIdentifier { get; set; }

		public string Name { get; set; }

		public string UrlHostName { get; set; }

		public string UrlSlug { get; set; }

		public string Description { get; set; }

		public bool Private { get; set; }

		public bool Moderated { get; set; }
	}
}