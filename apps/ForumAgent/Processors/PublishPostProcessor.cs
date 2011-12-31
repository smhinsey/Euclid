using System;
using System.Data.SqlTypes;
using Euclid.Common.Extensions;
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
		private readonly ISimpleRepository<ForumUserAction> _userActionRepository;
		private readonly ISimpleRepository<Forum> _forumRepository;
		private readonly ISimpleRepository<Category> _categoryRepository;

		public PublishPostProcessor(ISimpleRepository<Post> repository, ISimpleRepository<ForumUser> userRepository, ISimpleRepository<ModeratedPost> moderatedPostRepository, ISimpleRepository<Forum> forumRepository, ISimpleRepository<Category> categoryRepository, ISimpleRepository<ForumUserAction> userActionRepository)
		{
			_repository = repository;
			_userRepository = userRepository;
			_moderatedPostRepository = moderatedPostRepository;
			_forumRepository = forumRepository;
			_categoryRepository = categoryRepository;
			_userActionRepository = userActionRepository;
		}

		public override void Process(PublishPost message)
		{
			var user = _userRepository.FindById(message.AuthorIdentifier);

			var username = "Anonymous";

			if (user != null)
			{
				username = user.Username;
			}

			var forum = _forumRepository.FindById(message.ForumIdentifier);

			if (forum.Moderated)
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
											ApprovedOn = (DateTime)SqlDateTime.MinValue,
											Slug = message.Title.Slugify()
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
					Slug = message.Title.Slugify()
				};

				forum.TotalPosts++;

				_forumRepository.Save(forum);

				if (message.CategoryIdentifier != Guid.Empty)
				{
					var category = _categoryRepository.FindById(message.CategoryIdentifier);

					category.TotalPosts++;

					_categoryRepository.Save(category);
				}

				var userAction = new ForumUserAction()
					{
						Created = DateTime.Now,
						Modified = (DateTime)SqlDateTime.MinValue,
						UserIdentifier = message.AuthorIdentifier,
						ActivityOccurredOn = message.Created,
						AssociatedPostIdentifier = post.Identifier,
						AssociatedPostTitle = post.Title,
						Body = post.Body,
						ForumIdentifier = message.ForumIdentifier,
						IsPost = true
					};

				_userActionRepository.Save(userAction);

				_repository.Save(post);
			}

			if (user != null)
			{
				user.PostCount++;
				_userRepository.Update(user);
			}
		}
	}
}