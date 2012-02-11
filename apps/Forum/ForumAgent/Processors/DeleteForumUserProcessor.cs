using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class DeleteForumUserProcessor : DefaultCommandProcessor<DeleteForumUser>
	{
		private readonly ISimpleRepository<ForumUser> _repository;

		public DeleteForumUserProcessor(ISimpleRepository<ForumUser> repository)
		{
			_repository = repository;
		}

		public override void Process(DeleteForumUser message)
		{
			_repository.Delete(message.UserIdentifier);
		}
	}
}