using System;
using System.Collections.Generic;
using System.Linq;
using Euclid.Common.Storage.NHibernate;
using Euclid.Framework.Cqrs.NHibernate;
using ForumAgent.Domain.Entities;
using ForumAgent.ReadModels;
using NHibernate;

namespace ForumAgent.Queries
{
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
			       			Identifier = id,
			       			Modified = org.Modified,
			       			Name = org.OrganizationName,
			       			PhoneNumber = org.PhoneNumber,
			       			State = org.State,
			       			WebsiteUrl = org.OrganizationUrl,
			       			Zip = org.Zip,
			       			Slug = org.OrganizationSlug
			       		};
		}

		public Organization FindBySlug(string slug)
		{
			var session = GetCurrentSession();

			var org = session.QueryOver<OrganizationEntity>().Where(u => u.OrganizationSlug == slug).SingleOrDefault();

			return (org == null)
							? null
							: new Organization
							{
								Created = org.Created,
								Address = org.Address,
								Address2 = org.Address2,
								City = org.City,
								Country = org.Country,
								Identifier = org.Identifier,
								Modified = org.Modified,
								Name = org.OrganizationName,
								PhoneNumber = org.PhoneNumber,
								State = org.State,
								WebsiteUrl = org.OrganizationUrl,
								Zip = org.Zip,
								Slug = org.OrganizationSlug
							};
		}

		public new IList<Organization> List(int offset, int pageSize)
		{
			var session = GetCurrentSession();

			var orgs = session.QueryOver<OrganizationEntity>().Skip(offset).Take(pageSize);

			return
				orgs.List().Select(
					org =>
					new Organization
						{
							Created = org.Created,
							Address = org.Address,
							Address2 = org.Address2,
							City = org.City,
							Country = org.Country,
							Identifier = org.Identifier,
							Modified = org.Modified,
							Name = org.OrganizationName,
							PhoneNumber = org.PhoneNumber,
							State = org.State,
							WebsiteUrl = org.OrganizationUrl,
							Zip = org.Zip,
							Slug = org.OrganizationSlug
						}).ToList();
		}
	}
}