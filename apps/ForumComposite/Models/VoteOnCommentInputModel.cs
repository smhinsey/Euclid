using System;
using Euclid.Composites.Mvc.Models;

namespace ForumComposite.Models
{
	public class VoteOnCommentInputModel : InputModelBase
	{
		public Guid AuthorIdentifier { get; set; }

		public Guid CommentIdentifier { get; set; }

		public bool VoteUp { get; set; }
	}
}