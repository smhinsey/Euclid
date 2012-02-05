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
		private readonly NhSimpleRepository<OrganizationEntity> _repository;

		private readonly ISession _session;

		public UpdateOrganizationProcessor(ISession session)
		{
			_session = session;
			_repository = new NhSimpleRepository<OrganizationEntity>(_session);
		}

		public override void Process(UpdateOrganization command)
		{
			var organization = _repository.FindById(command.OrganizationIdentifier);

			if (organization == null)
			{
				throw new OrganizationNotFoundException(command.OrganizationIdentifier);
			}

			organization.Address = command.Address;
			organization.Address2 = command.Address2;
			organization.City = command.City;
			organization.Country = command.Country;
			organization.Modified = DateTime.Now;
			organization.OrganizationName = command.OrganizationName;
			organization.OrganizationUrl = command.OrganizationUrl;
			organization.PhoneNumber = command.PhoneNumber;
			organization.State = command.State;
			organization.Zip = command.Zip;
			organization.OrganizationSlug = command.OrganizationSlug.Trim();

			_repository.Save(organization);
		}
	}
}