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
	public class CommentOnPostProcessor : DefaultCommandProcessor<CommentOnPost>
	{
		private readonly ISimpleRepository<Comment> _commentRepository;

		private readonly ISimpleRepository<Post> _postRepository;

		private readonly ISimpleRepository<Forum> _forumRepository;

		private readonly ISimpleRepository<ForumUser> _userRepository;

		private readonly ISimpleRepository<ModeratedComment> _moderatedCommentRepository;
		private readonly ISimpleRepository<ForumUserAction> _userActionRepository;

		private readonly ProfanityFilterQueries _profanityFilterQueries;

		public CommentOnPostProcessor(
			ISimpleRepository<Comment> commentRepository,
			ISimpleRepository<Post> postRepository,
			ISimpleRepository<Forum> forumRepository,
			ISimpleRepository<ForumUser> userRepository, ISimpleRepository<ModeratedComment> moderatedCommentRepository, ISimpleRepository<ForumUserAction> userActionRepository, ProfanityFilterQueries profanityFilterQueries)
		{
			_commentRepository = commentRepository;
			_postRepository = postRepository;
			_forumRepository = forumRepository;
			_userRepository = userRepository;
			_moderatedCommentRepository = moderatedCommentRepository;
			_userActionRepository = userActionRepository;
			_profanityFilterQueries = profanityFilterQueries;
		}

		public override void Process(CommentOnPost message)
		{
			var user = _userRepository.FindById(message.AuthorIdentifier);

			var forum = _forumRepository.FindById(message.ForumIdentifier);

			var username = "Anonymous";

			if (user != null)
			{
				username = user.Username;
			}

			var profanityFilterStopWords = _profanityFilterQueries.FindAllActiveInForum(message.ForumIdentifier);

			var stopWordDictionary = new Dictionary<string, string>();

			foreach (var stopWord in profanityFilterStopWords)
			{
				if (!stopWordDictionary.ContainsKey(stopWord.WordToMatch))
				{
					stopWordDictionary.Add(stopWord.WordToMatch, stopWord.ReplacementWord);
				}
			}

			var post = _postRepository.FindById(message.PostIdentifier);

			if (forum.Moderated)
			{
				var comment = new ModeratedComment
				              	{
													ForumIdentifier = message.ForumIdentifier,
													AuthorIdentifier = message.AuthorIdentifier,
				              		AuthorDisplayName = username,
				              		Body = message.Body,
				              		PostIdentifier = message.PostIdentifier,
				              		Score = 0,
				              		Created = DateTime.Now,
				              		Modified = DateTime.Now,
				              		Title = message.Title,
				              		Approved = false,
				              		ApprovedBy = Guid.Empty,
				              		ApprovedOn = (DateTime) SqlDateTime.MinValue
				              	};

				_moderatedCommentRepository.Save(comment);
			}
			else
			{
				var comment = new Comment
				{
					ForumIdentifier = message.ForumIdentifier,
					AuthorIdentifier = message.AuthorIdentifier,
					AuthorDisplayName = username,
					Body = message.Body.Censor(stopWordDictionary),
					PostIdentifier = message.PostIdentifier,
					Score = 0,
					Created = DateTime.Now,
					Modified = DateTime.Now,
					Title = message.Title
				};

				_commentRepository.Save(comment);

				var userAction = new ForumUserAction()
				{
					Created = DateTime.Now,
					Modified = (DateTime)SqlDateTime.MinValue,
					UserIdentifier = message.AuthorIdentifier,
					ActivityOccurredOn = message.Created,
					AssociatedPostIdentifier = message.PostIdentifier,
					AssociatedPostTitle = post.Title,
					Body = message.Body.Censor(stopWordDictionary),
					ForumIdentifier = message.ForumIdentifier,
					IsComment = true
				};

				_userActionRepository.Save(userAction);
			}

			post.CommentCount++;

			_postRepository.Save(post);

			if (user != null)
			{
				user.CommentCount++;
				_userRepository.Update(user);
			}

		}
	}
}