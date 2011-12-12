using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Models
{
	public class DeleteForumUserInputModel : DefaultInputModel
	{
		public DeleteForumUserInputModel()
		{
			CommandType = typeof (DeleteForumUser);
		}

		public Guid UserIdentifier { get; set; }
	}
}