using System;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class UpdateUserProfileProcessor : DefaultCommandProcessor<UpdateUserProfile>
	{
		private readonly ISimpleRepository<UserProfile> _repository;

		public UpdateUserProfileProcessor(ISimpleRepository<UserProfile> repository)
		{
			_repository = repository;
		}

		public override void Process(UpdateUserProfile message)
		{
			var profile = new UserProfile
				{
					AvatarUrl = message.AvatarUrl,
					Email = message.Email,
					UserIdentifier = message.UserIdentifier,
					Created = DateTime.Now,
					Modified = DateTime.Now
				};

			_repository.Save(profile);
		}
	}
}