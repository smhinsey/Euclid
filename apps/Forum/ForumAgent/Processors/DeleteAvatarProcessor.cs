using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class DeleteAvatarProcessor : DefaultCommandProcessor<DeleteAvatar>
	{
		private readonly ISimpleRepository<ForumAvatar> _repository;

		public DeleteAvatarProcessor(ISimpleRepository<ForumAvatar> repository)
		{
			_repository = repository;
		}

		public override void Process(DeleteAvatar message)
		{
			_repository.Delete(message.AvatarIdentifier);
		}
	}
}