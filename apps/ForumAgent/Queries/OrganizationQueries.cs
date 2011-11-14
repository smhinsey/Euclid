using System;
using Euclid.Common.Storage.NHibernate;
using ForumAgent.Domain.Entities;
using ForumAgent.ReadModels;
using NHibernate;

namespace ForumAgent.Queries
{
	public class OrganizationQueries
	{
		private readonly ISession _session;
		private readonly NhSimpleRepository<OrganizationEntity> _repository;

		public OrganizationQueries(ISession session)
		{
			_session = session;
			_repository = new NhSimpleRepository<OrganizationEntity>(_session);
		}

		public Organization FindByIdentifier(Guid identifier)
		{
			var org = _repository.FindById(identifier);

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
			       	  		Zip = org.Zip
			       	  	};
		}
	}
}