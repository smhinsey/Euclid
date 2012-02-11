using System;
using System.Collections.Generic;
using System.Linq;
using Euclid.Framework.Cqrs.NHibernate;
using ForumAgent.Domain.Entities;
using ForumAgent.ReadModels;
using NHibernate;

namespace ForumAgent.Queries
{
	public class OrganizationUserQueries : NhQuery<OrganizationUser>
	{
		public OrganizationUserQueries(ISession session)
			: base(session)
		{
		}

		public bool AutenticateOrganizationUser(string username, string password)
		{
			var session = GetCurrentSession();

			// TODO: implement safe hashing/salting and all that noise
			var matchedAccount =
				session.QueryOver<OrganizationUserEntity>().Where(
					user => user.PasswordHash == password && user.PasswordSalt == password && user.Username == username && user.Active).
					SingleOrDefault();

			return matchedAccount != null;
		}

		public OrganizationUser FindByUsername(string username)
		{
			var session = GetCurrentSession();

			var user = session.QueryOver<OrganizationUserEntity>().Where(u => u.Username == username).SingleOrDefault();

			return (user == null)
			       	? null
			       	: new OrganizationUser
			       		{
			       			Created = user.Created,
			       			Email = user.Email,
			       			FirstName = user.FirstName,
			       			Identifier = user.Identifier,
			       			LastLogin = user.LastLogin,
			       			LastName = user.LastName,
			       			Modified = user.Modified,
			       			OrganizationIdentifier = user.OrganizationEntity.Identifier,
			       			Username = user.Username,
			       			PasswordSalt = user.PasswordHash,
			       			PasswordHash = user.PasswordSalt
			       		};
		}

		public OrganizationUsers FindByOrganization(Guid organizationId, int offset, int pageSize)
		{
			var session = GetCurrentSession();

			var users = session.QueryOver<OrganizationUserEntity>()
				.Where(u => u.OrganizationEntity.Identifier == organizationId)
				.Skip(offset).Take(pageSize).List();

			var org = session.QueryOver<OrganizationEntity>().Where(o => o.Identifier == organizationId).SingleOrDefault();
			return new OrganizationUsers
				{
					OrganizationName = org.OrganizationName,
					OrganizationIdentifier = organizationId,
					OrganizationSlug = org.OrganizationSlug,
					TotalNumberOfUsers = session.QueryOver<OrganizationUserEntity>().Where(o => o.OrganizationEntity.Identifier == organizationId).RowCount(),
					Users = users
						.Where(o => o.OrganizationEntity.Identifier == organizationId)
						.Skip(offset)
						.Take(pageSize)
						.Select(user =>
								new OrganizationUser
								{
									Created = user.Created,
									Email = user.Email,
									FirstName = user.FirstName,
									Identifier = user.Identifier,
									LastLogin = user.LastLogin,
									LastName = user.LastName,
									Modified = user.Modified,
									OrganizationIdentifier = user.OrganizationEntity.Identifier,
									Username = user.Username,
									PasswordSalt = user.PasswordHash,
									PasswordHash = user.PasswordSalt,
									Active = user.Active
								}).ToList()
				};
		}

		public new OrganizationUser FindById(Guid userId)
		{
			var session = GetCurrentSession();

			var user = session
						.QueryOver<OrganizationUserEntity>()
						.Where(u => u.Identifier == userId)
						.SingleOrDefault();

			return (user == null)
					? null
					: new OrganizationUser
					{
						Created = user.Created,
						Email = user.Email,
						FirstName = user.FirstName,
						Identifier = user.Identifier,
						LastLogin = user.LastLogin,
						LastName = user.LastName,
						Modified = user.Modified,
						OrganizationIdentifier = user.OrganizationEntity.Identifier,
						Username = user.Username,
						PasswordSalt = user.PasswordHash,
						PasswordHash = user.PasswordSalt
					};
		}
	}
}