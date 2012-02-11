using System;
using System.Data.SqlTypes;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class ApproveCommentProcessor : DefaultCommandProcessor<ApproveComment>
	{
		private readonly ISimpleRepository<ModeratedComment> _moderatedCommentRepository;
		private readonly ISimpleRepository<Comment> _commentRepository;

		public ApproveCommentProcessor(ISimpleRepository<Comment> commentRepository, ISimpleRepository<ModeratedComment> moderatedCommentRepository)
		{
			_commentRepository = commentRepository;
			_moderatedCommentRepository = moderatedCommentRepository;
		}

		public override void Process(ApproveComment message)
		{
			var moderatedComment = _moderatedCommentRepository.FindById(message.CommentIdentifier);

			if (moderatedComment == null)
			{
				throw new CommentNotFoundException(string.Format("Cannot approve comment with identifier '{0}'", message.CommentIdentifier));
			}

			moderatedComment.Approved = true;
			moderatedComment.ApprovedOn = DateTime.Now;
			moderatedComment.Modified = DateTime.Now;
			moderatedComment.ApprovedBy = message.ApprovedBy;

			_moderatedCommentRepository.Update(moderatedComment);

			_commentRepository.Save(new Comment
										{
											AuthorDisplayName = moderatedComment.AuthorDisplayName,
											AuthorIdentifier = moderatedComment.AuthorIdentifier,
											Body = moderatedComment.Body,
											Created = DateTime.Now,
											ForumIdentifier = moderatedComment.ForumIdentifier,
											Modified = (DateTime)SqlDateTime.MinValue,
											PostIdentifier = moderatedComment.PostIdentifier,
											Score = moderatedComment.Score,
											Title = moderatedComment.Title
										});
		}
	}
}