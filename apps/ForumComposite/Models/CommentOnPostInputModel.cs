using System;
using Euclid.Composites.Mvc.Models;

namespace ForumComposite.Models
{
	public class CommentOnPostInputModel : InputModelBase
	{
		public Guid AuthorIdentifier { get; set; }

		public string Body { get; set; }

		public Guid PostIdentifier { get; set; }

		public string Title { get; set; }
	}
}