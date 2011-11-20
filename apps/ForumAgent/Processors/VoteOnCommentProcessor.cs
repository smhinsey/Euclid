using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class VoteOnCommentProcessor : DefaultCommandProcessor<VoteOnComment>
	{
		private readonly ISimpleRepository<Comment> _repository;

		private readonly ISimpleRepository<ForumUser> _userRepository;

		public VoteOnCommentProcessor(ISimpleRepository<Comment> repository, ISimpleRepository<ForumUser> userRepository)
		{
			_repository = repository;
			_userRepository = userRepository;
		}

		public override void Process(VoteOnComment message)
		{
			var comment = _repository.FindById(message.CommentIdentifier);

			if (message.VoteUp)
			{
				comment.Score++;
			}
			else
			{
				comment.Score--;
			}

			_repository.Update(comment);

			var user = _userRepository.FindById(message.CreatedBy);
			if (user != null)
			{
				user.NumberVotes++;
				_userRepository.Update(user);
			}
		}
	}
}