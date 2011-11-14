using System;
using Euclid.Common.Storage.NHibernate;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.Domain.Entities;
using NHibernate;

namespace ForumAgent.Processors
{
	public class UpdateLastLoginProcessor : DefaultCommandProcessor<UpdateLastLogin>
	{
		private readonly ISession _session;
		private readonly NhSimpleRepository<OrganizationUserEntity> _repository;

		public UpdateLastLoginProcessor(ISession session)
		{
			_session = session;
			_repository = new NhSimpleRepository<OrganizationUserEntity>(_session);
		}

		public override void Process(UpdateLastLogin message)
		{
			var user = _session.QueryOver<OrganizationUserEntity>().Where(u => u.Identifier == message.UserIdentifier).SingleOrDefault();
			if (user == null)
			{
				throw new UserNotFoundException(message.UserIdentifier);
			}

			user.LastLogin = message.LoginTime;
			_repository.Update(user);
		}
	}

	public class UserNotFoundException : Exception
	{
		public UserNotFoundException (string name) : base(name)
		{
			
		}

		public UserNotFoundException (Guid id) : base(id.ToString())
		{
			
		}
	}
}