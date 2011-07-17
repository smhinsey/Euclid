using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class VoteOnCommentProcessor : DefaultCommandProcessor<VoteOnPost>
	{
		private readonly ISimpleRepository<Comment> _repository;

		public VoteOnCommentProcessor(ISimpleRepository<Comment> repository)
		{
			_repository = repository;
		}

		public override void Process(VoteOnPost message)
		{
			var comment = _repository.FindById(message.PostIdentifier);

			if (message.VoteUp)
			{
				comment.Score++;
			}
			else
			{
				comment.Score--;
			}

			_repository.Update(comment);
		}
	}
}