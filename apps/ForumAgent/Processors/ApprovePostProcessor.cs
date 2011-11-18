using System;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class ApprovePostProcessor : DefaultCommandProcessor<ApprovePost>
	{
		private readonly ISimpleRepository<ModeratedPost> _repository;

		private readonly ISimpleRepository<Post> _postRepository;

		public ApprovePostProcessor(ISimpleRepository<ModeratedPost> repository, ISimpleRepository<Post> postRepository)
		{
			_repository = repository;
			_postRepository = postRepository;
		}

		public override void Process(ApprovePost message)
		{
			var post = _repository.FindById(message.PostIdentifier);

			if (post == null)
			{
				throw new PostNotFoundException(string.Format("Unable to approve the post with id {0}", message.PostIdentifier));
			}

			post.Modified = DateTime.Now;
			post.Approved = true;
			post.ApprovedBy = message.ApprovedBy;
			post.ApprovedOn = DateTime.Now;
			_repository.Update(post);

			var approvedPost = new Post
			                   	{
			                   		AuthorDisplayName = post.AuthorDisplayName,
			                   		Created = post.Created,
			                   		AuthorIdentifier = post.AuthorIdentifier,
			                   		ForumIdentifier = post.ForumIdentifier,
			                   		CommentCount = post.CommentCount,
			                   		Body = post.Body,
			                   		CategoryIdentifier = post.CategoryIdentifier,
			                   		Modified = post.Modified,
			                   		Score = post.Score,
			                   		Title = post.Title
			                   	};
			_postRepository.Save(approvedPost);

		}
	}
}