using System;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class VoteOnPostProcessor : DefaultCommandProcessor<VoteOnPost>
	{
		private readonly ISimpleRepository<Post> _repository;
		private readonly ISimpleRepository<ForumUser> _userRepository;

		public VoteOnPostProcessor(ISimpleRepository<Post> repository, ISimpleRepository<ForumUser> userRepository)
		{
			_repository = repository;
			_userRepository = userRepository;
		}

		public override void Process(VoteOnPost message)
		{
			if(message.PostIdentifier == Guid.Empty)
			{
				return;
			}
		
			var post = _repository.FindById(message.PostIdentifier);

			if (message.VoteUp)
			{
				post.Score++;
			}
			else
			{
				post.Score--;
			}

			post.TotalVotes++;

			_repository.Update(post);

			var user = _userRepository.FindById(message.CreatedBy);
			if (user != null)
			{
				user.NumberVotes++;
				_userRepository.Update(user);
			}
		}
	}
}