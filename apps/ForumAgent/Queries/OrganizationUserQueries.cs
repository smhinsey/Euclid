using System;
using System.Collections.Generic;
using Euclid.Common.Storage.NHibernate;
using ForumAgent.Domain.Entities;
using ForumAgent.ReadModels;
using NHibernate;

namespace ForumAgent.Queries
{
	public class OrganizationUserQueries
	{
		private readonly ISession _session;
		private readonly NhSimpleRepository<OrganizationUserEntity> _repository;

		public OrganizationUserQueries(ISession session)
		{
			_session = session;
			_repository = new NhSimpleRepository<OrganizationUserEntity>(session);
			AutoMapper.Mapper.CreateMap<OrganizationUserEntity, OrganizationUser>();
		}

		public bool AutenticateOrganizationUser(string username, string password)
		{
			// TODO: implement safe hashing/salting and all that noise
			var matchedAccount = _session.QueryOver<OrganizationUserEntity>()
				.Where(
					user => user.PasswordHash == password &&
					        user.PasswordSalt == password &&
					        user.Username == username).SingleOrDefault();

			return matchedAccount != null;
		}

		public IList<OrganizationUser> List(int offset, int pageSize)
		{
			var domainUsers = _session.QueryOver<OrganizationUserEntity>().Skip(offset).Take(pageSize).List();

			return AutoMapper.Mapper.Map<IList<OrganizationUser>>(domainUsers);
		}

		public OrganizationUser FindByUsername(string username)
		{
			var user = _session
				.QueryOver<OrganizationUserEntity>()
				.Where(u => u.Username == username)
				.SingleOrDefault();

			if (user == null)
			{
				return null;
			}
			else
			{
				var orgUser = AutoMapper.Mapper.Map<OrganizationUser>(user);

				orgUser.OrganizationIdentifier = user.OrganizationEntity.Identifier;

				return orgUser;
			}
		}

		public OrganizationUser FindByIdentifier(Guid identifier)
		{
			var user = _session.QueryOver<OrganizationUserEntity>().Where(u => u.Identifier == identifier).SingleOrDefault();

			return (user == null) ? null : AutoMapper.Mapper.Map<OrganizationUser>(user);
		}
	}
}