using System;
using Euclid.Framework.Models;

namespace ForumAgent.ReadModels
{
	public class ForumUserFriend : DefaultReadModel
	{
		public virtual Guid ForumIdentifier { get; set; }
		public virtual Guid UserIdentifier { get; set; }
		public virtual string FriendUsername { get; set; }
		public virtual Guid FriendIdentifier { get; set; }
	}
}