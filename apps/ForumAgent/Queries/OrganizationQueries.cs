using System;
using Euclid.Common.Storage.NHibernate;
using Euclid.Framework.Cqrs.NHibernate;
using ForumAgent.Domain.Entities;
using ForumAgent.ReadModels;
using NHibernate;

namespace ForumAgent.Queries
{
	// queries need session management from NhQuery, even though the generic type isn't used currently
	// TODO: generic type should be an appropriate read model
	public class OrganizationQueries : NhQuery<Organization>
	{
		public OrganizationQueries(ISession session)
			: base(session)
		{
		}

		public override Organization FindById(Guid id)
		{
			var session = GetCurrentSession();

			var repository = new NhSimpleRepository<OrganizationEntity>(session);

			var org = repository.FindById(id);

			return (org == null)
			       	? null
			       	: new Organization
			       	  	{
			       	  		Created = org.Created,
			       	  		Address = org.Address,
			       	  		Address2 = org.Address2,
			       	  		City = org.City,
			       	  		Country = org.Country,
			       	  		Identifier = identifier,
			       	  		Modified = org.Modified,
			       	  		Name = org.OrganizationName,
			       	  		PhoneNumber = org.PhoneNumber,
			       	  		State = org.State,
			       	  		WebsiteUrl = org.OrganizationUrl,
			       	  		Zip = org.Zip,
							Slug = org.OrganizationSlug
			       	  	};
		}

		public Guid GetIdentifierBySlug(string slug)
		{
			var session = GetCurrentSession();

			var org = session.QueryOver<OrganizationEntity>().Where(u => u.OrganizationSlug == slug).SingleOrDefault();

			return org.Identifier;
		}
	}
}