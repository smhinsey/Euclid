using System;
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

		private readonly ISimpleRepository<User> _userRepository;

		public CommentOnPostProcessor(ISimpleRepository<Comment> commentRepository, ISimpleRepository<Post> postRepository, ISimpleRepository<User> userRepository)
		{
			_commentRepository = commentRepository;
			_postRepository = postRepository;
			_userRepository = userRepository;
		}

		public override void Process(CommentOnPost message)
		{
			var user = _userRepository.FindById(message.AuthorIdentifier);

			var comment = new Comment
				{
					AuthorIdentifier = message.AuthorIdentifier,
					AuthorDisplayName = user.Username,
					Body = message.Body,
					PostIdentifier = message.PostIdentifier,
					Score = 0,
					Created = DateTime.Now,
					Modified = DateTime.Now,
					Title = message.Title
				};
			
			_commentRepository.Save(comment);

			var post = _postRepository.FindById(message.PostIdentifier);

			post.CommentCount++;

			_postRepository.Save(post);
		}
	}
}