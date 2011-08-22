using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class VoteOnCommentProcessor : DefaultCommandProcessor<VoteOnComment>
	{
		private readonly ISimpleRepository<Comment> _repository;

		public VoteOnCommentProcessor(ISimpleRepository<Comment> repository)
		{
			this._repository = repository;
		}

		public override void Process(VoteOnComment message)
		{
			var comment = this._repository.FindById(message.CommentIdentifier);

			if (message.VoteUp)
			{
				comment.Score++;
			}
			else
			{
				comment.Score--;
			}

			this._repository.Update(comment);
		}
	}
}