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

			return (user == null)
			       	? null
			       	: new OrganizationUser
			       	  	{
			       	  		Created = user.Created,
			       	  		Email = user.Email,
			       	  		FirstName = user.FirstName,
			       	  		LastName = user.LastName,
			       	  		Identifier = user.Identifier,
			       	  		LastLogin = user.LastLogin,
			       	  		Modified = user.Modified,
			       	  		OrganizationIdentifier = user.OrganizationEntity.Identifier,
			       	  		PasswordHash = user.PasswordHash,
			       	  		PasswordSalt = user.PasswordSalt,
			       	  		Username = user.Username
			       	  	};
		}
	}
}