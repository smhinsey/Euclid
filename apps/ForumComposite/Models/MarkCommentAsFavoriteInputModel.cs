using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace ForumComposite.Models
{
	public class MarkCommentAsFavoriteInputModel : DefaultInputModel
	{
		public MarkCommentAsFavoriteInputModel()
		{
			CommandType = typeof(MarkCommentAsFavorite);
		}

		public Guid ForumIdentifier { get; set; }
		public Guid PostIdentifier { get; set; }
		public Guid CommentIdentifier { get; set; }
		public Guid UserIdentifier { get; set; }	 
	}
}