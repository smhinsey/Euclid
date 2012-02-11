using System;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class UpdateAvatarProcessor : DefaultCommandProcessor<UpdateAvatar>
	{
		private readonly ISimpleRepository<ForumAvatar> _repository;

		public UpdateAvatarProcessor(ISimpleRepository<ForumAvatar> repository)
		{
			_repository = repository;
		}

		public override void Process(UpdateAvatar message)
		{
			var avatar         = _repository.FindById(message.AvatarIdentifier);
			avatar.Active      = false;
			avatar.Modified    = DateTime.Now;
			avatar.Description = message.Description;
			avatar.Name        = message.Name;
			avatar.Url         = message.ImageUrl;

			_repository.Update(avatar);
		}
	}
}