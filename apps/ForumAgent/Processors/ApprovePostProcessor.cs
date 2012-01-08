using System;
using System.Data.SqlTypes;
using Euclid.Common.Extensions;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.Queries;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class ApprovePostProcessor : DefaultCommandProcessor<ApprovePost>
	{
		private readonly ISimpleRepository<Category> _categoryRepository;

		private readonly ISimpleRepository<Forum> _forumRepository;

		private readonly ISimpleRepository<Post> _postRepository;

		private readonly ISimpleRepository<ModeratedPost> _repository;

		private readonly TagQueries _tagQueries;

		private readonly ISimpleRepository<Tag> _tagRepository;

		public ApprovePostProcessor(
			ISimpleRepository<ModeratedPost> repository,
			ISimpleRepository<Post> postRepository,
			ISimpleRepository<Forum> forumRepository,
			ISimpleRepository<Category> categoryRepository, ISimpleRepository<Tag> tagRepository, TagQueries tagQueries)
		{
			_repository = repository;
			_postRepository = postRepository;
			_forumRepository = forumRepository;
			_categoryRepository = categoryRepository;
			_tagRepository = tagRepository;
			_tagQueries = tagQueries;
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
					Title = post.Title,
					Slug = post.Slug,
					Tags = post.Tags
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

			var tags = post.Tags.Split(new[] { "," }, StringSplitOptions.None);

			foreach (var tag in tags)
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

			_postRepository.Save(approvedPost);
		}
	}
}