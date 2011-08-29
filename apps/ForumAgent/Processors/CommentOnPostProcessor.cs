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

		public CommentOnPostProcessor(ISimpleRepository<Comment> commentRepository, ISimpleRepository<Post> postRepository)
		{
			_commentRepository = commentRepository;
			_postRepository = postRepository;
		}

		public override void Process(CommentOnPost message)
		{
			// SELF at some point we'll want to wire up the actual author name there
			var comment = new Comment
				{
					AuthorIdentifier = message.AuthorIdentifier,
					AuthorDisplayName = "Anonymous",
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