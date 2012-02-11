using System;
using Euclid.Common.Storage.NHibernate;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.Domain.Entities;
using NHibernate;

namespace ForumAgent.Processors
{
	public class UpdateOrganizationUserLastLoginProcessor : DefaultCommandProcessor<UpdateOrganizationUserLastLogin>
	{
		private readonly NhSimpleRepository<OrganizationUserEntity> _repository;

		private readonly ISession _session;

		public UpdateOrganizationUserLastLoginProcessor(ISession session)
		{
			_session = session;
			_repository = new NhSimpleRepository<OrganizationUserEntity>(_session);
		}

		public override void Process(UpdateOrganizationUserLastLogin message)
		{
			var user = _repository.FindById(message.UserIdentifier);
			if (user == null)
			{
				throw new UserNotFoundException(message.UserIdentifier);
			}

			user.LastLogin = message.LoginTime;
			user.Modified = DateTime.Now;
			_repository.Update(user);
		}
	}
}