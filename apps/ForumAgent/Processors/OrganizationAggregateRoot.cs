using System;
using Euclid.Common.Messaging;
using Euclid.Common.Storage.Model;
using ForumAgent.Commands;
using ForumAgent.WriteModels;

namespace ForumAgent.Processors
{
	public class OrganizationAggregateRoot : MultipleMessageProcessor
	{

		private readonly ISimpleRepository<Organization> _organizationRepository;
		private readonly ISimpleRepository<OrganizationUser> _organizationUserRepository;

		public OrganizationAggregateRoot(ISimpleRepository<Organization> organizationRepository, ISimpleRepository<OrganizationUser> organizationUserRepository)
		{
			_organizationRepository = organizationRepository;
			_organizationUserRepository = organizationUserRepository;
		}

		public void CreateOrganization(CreateOrganization createOrganization)
		{
			AutoMapper.Mapper.CreateMap<CreateOrganization, Organization>();

			var writeModel = AutoMapper.Mapper.Map<Organization>(createOrganization);
			writeModel.Created = DateTime.Now;
			writeModel.Modified = DateTime.Now;

			_organizationRepository.Save(writeModel);
		}

		public void RegisterOrganizationUser(RegisterOrganizationUser registerOrganizationUser)
		{
			AutoMapper.Mapper.CreateMap<RegisterOrganizationUser, OrganizationUser>();

			var writeModel = AutoMapper.Mapper.Map<OrganizationUser>(registerOrganizationUser);

			writeModel.Organization = _organizationRepository.FindById(registerOrganizationUser.OrganizationId);
			writeModel.Created = DateTime.Now;
			writeModel.Modified = DateTime.Now;

			_organizationUserRepository.Save(writeModel);
		}
	}
}