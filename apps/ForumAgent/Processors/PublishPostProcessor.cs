using System;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class PublishPostProcessor : DefaultCommandProcessor<PublishPost>
	{
		private readonly ISimpleRepository<Post> _repository;

		private readonly ISimpleRepository<User> _userRepository;

		public PublishPostProcessor(ISimpleRepository<Post> repository, ISimpleRepository<User> userRepository)
		{
			_repository = repository;
			_userRepository = userRepository;
		}

		public override void Process(PublishPost message)
		{
			var user = _userRepository.FindById(message.AuthorIdentifier);

			var post = new Post
				{
					AuthorIdentifier = message.AuthorIdentifier,
					AuthorDisplayName = user.Username,
					Body = message.Body,
					Score = 0,
					Title = message.Title,
					CategoryIdentifier = message.CategoryIdentifier,
					Identifier = message.Identifier,
					Created = DateTime.Now,
					Modified = DateTime.Now
				};

			_repository.Save(post);
		}
	}
}