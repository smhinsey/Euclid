using System;
using System.Collections.Generic;
using Euclid.Common.Storage.NHibernate;
using ForumAgent.ReadModels;
using ForumAgent.WriteModels;
using NHibernate;

namespace ForumAgent.Queries
{
	public class OrganizationUserQueries
	{
		private readonly ISession _session;
		private readonly NhSimpleRepository<DomainOrganizationUser> _repository;

		public OrganizationUserQueries(ISession session)
		{
			_session = session;
			_repository = new NhSimpleRepository<DomainOrganizationUser>(session);
			AutoMapper.Mapper.CreateMap<DomainOrganizationUser, OrganizationUser>();
		}

		public bool AutenticateOrganizationUser(string username, string password)
		{
			// TODO: implement safe hashing/salting and all that noise
			var matchedAccount = _session.QueryOver<DomainOrganizationUser>()
				.Where(
					user => user.PasswordHash == password &&
					        user.PasswordSalt == password &&
					        user.Username == username).SingleOrDefault();

			return matchedAccount != null;
		}

		public IList<OrganizationUser> List(int offset, int pageSize)
		{
			var domainUsers = _session.QueryOver<DomainOrganizationUser>().Skip(offset).Take(pageSize).List();

			return AutoMapper.Mapper.Map<IList<OrganizationUser>>(domainUsers);
		}

		public OrganizationUser FindByUsername(string username)
		{
			var user = _session
				.QueryOver<DomainOrganizationUser>()
				.Where(u => u.Username == username)
				.SingleOrDefault();

			return (user == null) ? null : AutoMapper.Mapper.Map<OrganizationUser>(user);
		}

		public OrganizationUser FindByIdentifier(Guid identifier)
		{
			var user = _session.QueryOver<DomainOrganizationUser>().Where(u => u.Identifier == identifier).SingleOrDefault();

			return (user == null) ? null : AutoMapper.Mapper.Map<OrganizationUser>(user);
		}
	}
}