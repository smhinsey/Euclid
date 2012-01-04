using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.Queries;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class RemoveForumUserFriendProcessor : DefaultCommandProcessor<RemoveForumUserFriend>
	{
		private readonly ISimpleRepository<ForumUserFriend> _userFriendRepository;

		private readonly UserQueries _userQueries;

		public RemoveForumUserFriendProcessor(ISimpleRepository<ForumUserFriend> userFriendRepository, UserQueries userQueries)
		{
			_userFriendRepository = userFriendRepository;
			_userQueries = userQueries;
		}

		public override void Process(RemoveForumUserFriend message)
		{
			var friends = _userQueries.FindUserFriends(message.ForumIdentifier, message.UserIdentifier);

			foreach (var friend in friends)
			{
				if (friend.FriendIdentifier == message.FriendIdentifier)
				{
					_userFriendRepository.Delete(friend.Identifier);
				}
			}
		}
	}
}