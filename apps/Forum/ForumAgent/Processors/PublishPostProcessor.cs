using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Euclid.Common.Extensions;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.Queries;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class PublishPostProcessor : DefaultCommandProcessor<PublishPost>
	{
		private readonly ISimpleRepository<Category> _categoryRepository;

		private readonly ISimpleRepository<Forum> _forumRepository;

		private readonly ISimpleRepository<ModeratedPost> _moderatedPostRepository;

		private readonly ISimpleRepository<Post> _repository;

		private readonly TagQueries _tagQueries;
		private readonly ProfanityFilterQueries _profanityFilterQueries;

		private readonly ISimpleRepository<Tag> _tagRepository;

		private readonly ISimpleRepository<ForumUserAction> _userActionRepository;

		private readonly ISimpleRepository<ForumUser> _userRepository;

		public PublishPostProcessor(
			ISimpleRepository<Post> repository,
			ISimpleRepository<ForumUser> userRepository,
			ISimpleRepository<ModeratedPost> moderatedPostRepository,
			ISimpleRepository<Forum> forumRepository,
			ISimpleRepository<Category> categoryRepository,
			ISimpleRepository<ForumUserAction> userActionRepository,
			TagQueries tagQueries,
			ISimpleRepository<Tag> tagRepository, ProfanityFilterQueries profanityFilterQueries)
		{
			_repository = repository;
			_userRepository = userRepository;
			_moderatedPostRepository = moderatedPostRepository;
			_forumRepository = forumRepository;
			_categoryRepository = categoryRepository;
			_userActionRepository = userActionRepository;
			_tagQueries = tagQueries;
			_tagRepository = tagRepository;
			_profanityFilterQueries = profanityFilterQueries;
		}

		public override void Process(PublishPost message)
		{
			var user = _userRepository.FindById(message.AuthorIdentifier);
			var category = _categoryRepository.FindById(message.CategoryIdentifier);

			var profanityFilterStopWords = _profanityFilterQueries.FindAllActiveInForum(message.ForumIdentifier);

			var stopWordDictionary = new Dictionary<string, string>();

			foreach (var stopWord in profanityFilterStopWords)
			{
				if(!stopWordDictionary.ContainsKey(stopWord.WordToMatch))
				{
					stopWordDictionary.Add(stopWord.WordToMatch, stopWord.ReplacementWord);
				}
			}

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
						Modified = (DateTime)SqlDateTime.MinValue,
						ForumIdentifier = message.ForumIdentifier,
						Approved = false,
						ApprovedOn = (DateTime)SqlDateTime.MinValue,
						Slug = message.Title.Slugify(),
						Tags = string.Join(", ", message.Tags)
					};

				_moderatedPostRepository.Save(post);
			}
			else
			{
				var post = new Post
					{
						AuthorIdentifier = message.AuthorIdentifier,
						AuthorDisplayName = username,
						Body = message.Body.Censor(stopWordDictionary),
						Score = 0,
						Title = message.Title.Censor(stopWordDictionary),
						CategoryName = category.Name,
						CategorySlug = category.Slug,
						CategoryIdentifier = message.CategoryIdentifier,
						Identifier = message.Identifier,
						Created = DateTime.Now,
						Modified = (DateTime)SqlDateTime.MinValue,
						ForumIdentifier = message.ForumIdentifier,
						Slug = message.Title.Slugify(),
						Tags = string.Join(", ", message.Tags)
					};

				forum.TotalPosts++;

				_forumRepository.Save(forum);

				if (message.CategoryIdentifier != Guid.Empty)
				{
					category.TotalPosts++;

					_categoryRepository.Save(category);
				}

				foreach (var tag in message.Tags)
				{
					var tagRecord = _tagQueries.FindByName(forum.Identifier, tag);

					if (tagRecord == null)
					{
						tagRecord = new Tag
							{
								Identifier = Guid.NewGuid(),
								ForumIdentifier = forum.Identifier,
								Name = tag.Slugify(),
								TotalPosts = 1,
								Created = DateTime.Now,
								Modified = (DateTime)SqlDateTime.MinValue,
								Active = true,
							};

						_tagRepository.Save(tagRecord);
					}
					else
					{
						tagRecord.TotalPosts++;

						_tagRepository.Update(tagRecord);
					}
				}

				_repository.Save(post);

				var userAction = new ForumUserAction
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
			}

			if (user != null)
			{
				user.PostCount++;
				_userRepository.Update(user);
			}
		}
	}
}