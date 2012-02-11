using System;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.Domain.Entities;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class ActivateOrganizationUserProcessor : DefaultCommandProcessor<ActivateOrganizationUser>
	{
		private readonly ISimpleRepository<OrganizationUserEntity> _repository;

		public ActivateOrganizationUserProcessor(ISimpleRepository<OrganizationUserEntity> repository)
		{
			_repository = repository;
		}

		public override void Process(ActivateOrganizationUser message)
		{
			var user = _repository.FindById(message.UserIdentifier);

			user.Active = message.Active;
			user.Modified = DateTime.Now;

			_repository.Update(user);
		}
	}
}