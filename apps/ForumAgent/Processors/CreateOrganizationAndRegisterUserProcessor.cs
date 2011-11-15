using System;
using System.Data.SqlTypes;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.Domain.Entities;

namespace ForumAgent.Processors
{
	public class CreateOrganizationAndRegisterUserProcessor : DefaultCommandProcessor<CreateOrganizationAndRegisterUser>
	{

		private readonly ISimpleRepository<OrganizationEntity> _organizationRepository;
		private readonly ISimpleRepository<OrganizationUserEntity> _organizationUserRepository;

		public CreateOrganizationAndRegisterUserProcessor(ISimpleRepository<OrganizationEntity> organizationRepository, ISimpleRepository<OrganizationUserEntity> organizationUserRepository)
		{
			_organizationRepository = organizationRepository;
			_organizationUserRepository = organizationUserRepository;
		}

		public override void Process(CreateOrganizationAndRegisterUser message)
		{
			AutoMapper.Mapper.CreateMap<CreateOrganizationAndRegisterUser, OrganizationEntity>();
			AutoMapper.Mapper.CreateMap<CreateOrganizationAndRegisterUser, OrganizationUserEntity>();
			DateTime created = DateTime.Now;

			var organizationUserWriteModel = AutoMapper.Mapper.Map<OrganizationUserEntity>(message);
			organizationUserWriteModel.Created = created;
			organizationUserWriteModel.Modified = created;
			organizationUserWriteModel.LastLogin = (DateTime)SqlDateTime.MinValue;
			organizationUserWriteModel.CreatedBy = Guid.Empty;

			var organizationWriteModel = AutoMapper.Mapper.Map<OrganizationEntity>(message);
			organizationWriteModel.Created = created;
			organizationWriteModel.Modified = created;

			organizationUserWriteModel.OrganizationEntity = _organizationRepository.Save(organizationWriteModel);
			var user = _organizationUserRepository.Save(organizationUserWriteModel);

			organizationWriteModel.CreatedBy = user.Identifier;
			_organizationRepository.Update(organizationWriteModel);
		}
	}
}