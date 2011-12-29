using System;
using System.Data.SqlTypes;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class CommentOnPostProcessor : DefaultCommandProcessor<CommentOnPost>
	{
		private readonly ISimpleRepository<Comment> _commentRepository;

		private readonly ISimpleRepository<Post> _postRepository;

		private readonly ISimpleRepository<ForumUser> _userRepository;

		private readonly ISimpleRepository<ModeratedComment> _moderatedCommentRepository;

		public CommentOnPostProcessor(
			ISimpleRepository<Comment> commentRepository,
			ISimpleRepository<Post> postRepository,
			ISimpleRepository<ForumUser> userRepository, ISimpleRepository<ModeratedComment> moderatedCommentRepository)
		{
			_commentRepository = commentRepository;
			_postRepository = postRepository;
			_userRepository = userRepository;
			_moderatedCommentRepository = moderatedCommentRepository;
		}

		public override void Process(CommentOnPost message)
		{
			var user = _userRepository.FindById(message.AuthorIdentifier);

			var username = "Anonymous";

			if (user != null)
			{
				username = user.Username;
			}

			if (message.ModerationRequired)
			{
				var comment = new ModeratedComment
				              	{
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
					AuthorIdentifier = message.AuthorIdentifier,
					AuthorDisplayName = username,
					Body = message.Body,
					PostIdentifier = message.PostIdentifier,
					Score = 0,
					Created = DateTime.Now,
					Modified = DateTime.Now,
					Title = message.Title
				};

				_commentRepository.Save(comment);
			}

			var post = _postRepository.FindById(message.PostIdentifier);

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