using System;
using Euclid.Common.Storage.NHibernate;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.Domain.Entities;
using NHibernate;

namespace ForumAgent.Processors
{
	public class UpdateOrganizationProcessor : DefaultCommandProcessor<UpdateOrganization>
	{
		private readonly ISession _session;
		private readonly NhSimpleRepository<OrganizationEntity> _repository;

		public UpdateOrganizationProcessor(ISession session)
		{
			_session = session;
			_repository = new NhSimpleRepository<OrganizationEntity>(_session);
		}

		public override void Process(UpdateOrganization organizationInfo)
		{
			var entity = _repository.FindById(organizationInfo.OrganizationIdentifier);

			if (entity == null)
			{
				throw new OrganizationNotFoundException(organizationInfo.OrganizationIdentifier);
			}

			entity.Address = organizationInfo.Address;
			entity.Address2 = organizationInfo.Address2;
			entity.City = organizationInfo.City;
			entity.Country = organizationInfo.Country;
			entity.Modified = DateTime.Now;
			entity.OrganizationName = organizationInfo.OrganizationName;
			entity.OrganizationUrl = organizationInfo.OrganizationUrl;
			entity.PhoneNumber = organizationInfo.PhoneNumber;
			entity.State = organizationInfo.State;
			entity.Zip = organizationInfo.Zip;
			entity.OrganizationSlug = organizationInfo.OrganizationSlug;

			_repository.Save(entity);
		}
	}
}