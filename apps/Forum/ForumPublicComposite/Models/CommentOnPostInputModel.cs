using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace ForumComposite.Models
{
	public class CommentOnPostInputModel : DefaultInputModel
	{
		public CommentOnPostInputModel()
		{
			CommandType = typeof(CommentOnPost);
		}

		public Guid ForumIdentifier { get; set; }

		public Guid AuthorIdentifier { get; set; }

		public string Body { get; set; }

		public Guid PostIdentifier { get; set; }

		public string Title { get; set; }
	}
}