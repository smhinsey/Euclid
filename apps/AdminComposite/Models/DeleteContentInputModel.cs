using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Models
{
	public class DeleteContentInputModel : DefaultInputModel
	{
		public DeleteContentInputModel()
		{
			CommandType = typeof (DeleteForumContent);
		}

		public Guid ContentIdentifier { get; set; }
	}
}