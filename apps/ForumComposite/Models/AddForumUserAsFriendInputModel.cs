using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace ForumComposite.Models
{
	public class AddForumUserAsFriendInputModel : DefaultInputModel
	{
		public AddForumUserAsFriendInputModel()
		{
			CommandType = typeof(AddForumUserAsFriend);
		}

		public Guid ForumIdentifier { get; set; }
		public Guid UserIdentifier { get; set; }
		public Guid FriendIdentifier { get; set; }	 
	}
}