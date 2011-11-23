using System;
using System.Data.SqlTypes;
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
		private readonly ISimpleRepository<Forum> _forumRepository;
		private readonly ISimpleRepository<Category> _categoryRepository;

		public ApprovePostProcessor(ISimpleRepository<ModeratedPost> repository, ISimpleRepository<Post> postRepository, ISimpleRepository<Forum> forumRepository, ISimpleRepository<Category> categoryRepository)
		{
			_repository = repository;
			_postRepository = postRepository;
			_forumRepository = forumRepository;
			_categoryRepository = categoryRepository;
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
									Identifier = post.Identifier,
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

			var forum = _forumRepository.FindById(post.ForumIdentifier);

			forum.TotalPosts++;

			_forumRepository.Save(forum);

			if (post.CategoryIdentifier != Guid.Empty)
			{
				var category = _categoryRepository.FindById(post.CategoryIdentifier);

				category.TotalPosts++;

				_categoryRepository.Save(category);
			}

			_postRepository.Save(approvedPost);

		}
	}
}