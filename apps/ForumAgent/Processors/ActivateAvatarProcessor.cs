using System;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class ActivateAvatarProcessor : DefaultCommandProcessor<ActivateAvatar>
	{
		private readonly ISimpleRepository<ForumAvatar> _repository;

		public ActivateAvatarProcessor(ISimpleRepository<ForumAvatar> repository)
		{
			_repository = repository;
		}

		public override void Process(ActivateAvatar message)
		{
			var avatar = _repository.FindById(message.AvatarIdentifier);

			avatar.Active = message.Active;
			avatar.Modified = DateTime.Now;

			_repository.Update(avatar);
		}
	}
}