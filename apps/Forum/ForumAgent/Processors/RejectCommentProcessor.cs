using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class RejectCommentProcessor : DefaultCommandProcessor<RejectComment>
	{
		private readonly ISimpleRepository<ModeratedComment> _repository;

		public RejectCommentProcessor(ISimpleRepository<ModeratedComment> repository)
		{
			_repository = repository;
		}

		public override void Process(RejectComment message)
		{
			_repository.Delete(message.Identifier);
		}
	}
}