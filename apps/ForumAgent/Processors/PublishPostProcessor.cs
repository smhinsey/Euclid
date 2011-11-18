using System;
using System.Data.SqlTypes;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class PublishPostProcessor : DefaultCommandProcessor<PublishPost>
	{
		private readonly ISimpleRepository<Post> _repository;

		private readonly ISimpleRepository<ModeratedPost> _moderatedPostRepository;

		private readonly ISimpleRepository<ForumUser> _userRepository;

		public PublishPostProcessor(ISimpleRepository<Post> repository, ISimpleRepository<ForumUser> userRepository, ISimpleRepository<ModeratedPost> moderatedPostRepository)
		{
			_repository = repository;
			_userRepository = userRepository;
			_moderatedPostRepository = moderatedPostRepository;
		}

		public override void Process(PublishPost message)
		{
			var user = _userRepository.FindById(message.AuthorIdentifier);

			var username = "Anonymous";

			if (user != null)
			{
				username = user.Username;
			}

			if (message.ModerationRequired)
			{
				var post = new ModeratedPost
				           	{
				           		AuthorIdentifier = message.AuthorIdentifier,
				           		AuthorDisplayName = username,
				           		Body = message.Body,
				           		Score = 0,
				           		Title = message.Title,
				           		CategoryIdentifier = message.CategoryIdentifier,
				           		Identifier = message.Identifier,
				           		Created = DateTime.Now,
				           		Modified = (DateTime) SqlDateTime.MinValue,
				           		ForumIdentifier = message.ForumIdentifier,
								Approved = false,
								ApprovedOn = (DateTime)SqlDateTime.MinValue
				           	};

				_moderatedPostRepository.Save(post);
			}
			else
			{
				var post = new Post
				{
					AuthorIdentifier = message.AuthorIdentifier,
					AuthorDisplayName = username,
					Body = message.Body,
					Score = 0,
					Title = message.Title,
					CategoryIdentifier = message.CategoryIdentifier,
					Identifier = message.Identifier,
					Created = DateTime.Now,
					Modified = (DateTime)SqlDateTime.MinValue,
					ForumIdentifier = message.ForumIdentifier,
				};

				_repository.Save(post);
			}
		}
	}
}