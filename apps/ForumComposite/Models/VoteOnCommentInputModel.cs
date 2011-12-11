using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace ForumComposite.Models
{
	public class VoteOnCommentInputModel : DefaultInputModel
	{
		public VoteOnCommentInputModel()
		{
			CommandType = typeof(VoteOnComment);
		}

		public Guid AuthorIdentifier { get; set; }

		public Guid CommentIdentifier { get; set; }

		public bool VoteUp { get; set; }
	}
}