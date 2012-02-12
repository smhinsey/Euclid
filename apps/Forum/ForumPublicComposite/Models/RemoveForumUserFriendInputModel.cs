using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace ForumComposite.Models
{
	public class RemoveForumUserFriendInputModel : DefaultInputModel
	{
		public RemoveForumUserFriendInputModel()
		{
			CommandType = typeof(RemoveForumUserFriend);
		}

		public Guid ForumIdentifier { get; set; }
		public Guid UserIdentifier { get; set; }
		public Guid FriendIdentifier { get; set; }	 
	}
}