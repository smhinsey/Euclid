using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace ForumComposite.Models
{
	public class CommentOnPostInputModel : InputModelBase
	{
		public CommentOnPostInputModel()
		{
			AgentSystemName = "NewCo.ForumAgent";
			CommandType = typeof(CommentOnPost);
		}

		public Guid AuthorIdentifier { get; set; }

		public string Body { get; set; }

		public Guid PostIdentifier { get; set; }

		public string Title { get; set; }
	}
}