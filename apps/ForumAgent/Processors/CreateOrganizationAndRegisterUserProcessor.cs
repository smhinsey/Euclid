using System;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.WriteModels;

namespace ForumAgent.Processors
{
	public class CreateOrganizationAndRegisterUserProcessor : DefaultCommandProcessor<CreateOrganizationAndRegisterUser>
	{

		private readonly ISimpleRepository<DomainOrganization> _organizationRepository;
		private readonly ISimpleRepository<DomainOrganizationUser> _organizationUserRepository;

		public CreateOrganizationAndRegisterUserProcessor(ISimpleRepository<DomainOrganization> organizationRepository, ISimpleRepository<DomainOrganizationUser> organizationUserRepository)
		{
			_organizationRepository = organizationRepository;
			_organizationUserRepository = organizationUserRepository;
		}

		public override void Process(CreateOrganizationAndRegisterUser message)
		{
			AutoMapper.Mapper.CreateMap<CreateOrganizationAndRegisterUser, DomainOrganization>();
			AutoMapper.Mapper.CreateMap<CreateOrganizationAndRegisterUser, DomainOrganizationUser>();
			DateTime created = DateTime.Now;

			var organizationUserWriteModel = AutoMapper.Mapper.Map<DomainOrganizationUser>(message);
			organizationUserWriteModel.Created = created;
			organizationUserWriteModel.Modified = created;

			var organizationWriteModel = AutoMapper.Mapper.Map<DomainOrganization>(message);
			organizationWriteModel.Created = created;
			organizationWriteModel.Modified = created;

			organizationUserWriteModel.Organization = _organizationRepository.Save(organizationWriteModel);
			_organizationUserRepository.Save(organizationUserWriteModel);
		}
	}
}