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
					user => user.PasswordHash == password && user.PasswordSalt == password && user.Username == username).
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

		public new IList<OrganizationUser> List(int offset, int pageSize)
		{
			var session = GetCurrentSession();

			var users = session.QueryOver<OrganizationUserEntity>().Skip(offset).Take(pageSize).List();

			return
				users.Select(
					user =>
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
							PasswordHash = user.PasswordSalt
						}).ToList();
		}
	}
}