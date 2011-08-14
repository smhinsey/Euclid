using System;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class CommentOnPostProcessor : DefaultCommandProcessor<CommentOnPost>
	{
		private readonly ISimpleRepository<Comment> _repository;

		public CommentOnPostProcessor(ISimpleRepository<Comment> repository)
		{
			_repository = repository;
		}

		public override void Process(CommentOnPost message)
		{
			var comment = new Comment
			              	{
			              		AuthorIdentifier = message.AuthorIdentifier,
			              		Body = message.Body,
			              		PostIdentifier = message.PostIdentifier,
			              		Score = 0,
			              		Created = DateTime.Now,
			              		Modified = DateTime.Now
			              	};

			_repository.Save(comment);
		}
	}
}