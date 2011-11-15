using System;
using System.Collections.Generic;
using Euclid.Common.Storage.NHibernate;
using Euclid.Framework.Cqrs.NHibernate;
using ForumAgent.Domain.Entities;
using ForumAgent.ReadModels;
using NHibernate;

namespace ForumAgent.Queries
{
	public class OrganizationUserQueries : NhQuery<OrganizationUser>
	{

		public OrganizationUserQueries(ISession session) : base(session)
		{
		}

		public bool AutenticateOrganizationUser(string username, string password)
		{
			var session = GetCurrentSession();

			// TODO: implement safe hashing/salting and all that noise
			var matchedAccount = session.QueryOver<OrganizationUserEntity>()
				.Where(
					user => user.PasswordHash == password &&
					        user.PasswordSalt == password &&
					        user.Username == username).SingleOrDefault();

			return matchedAccount != null;
		}

		public OrganizationUser FindByUsername(string username)
		{
			var session = GetCurrentSession();

			var user = session
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