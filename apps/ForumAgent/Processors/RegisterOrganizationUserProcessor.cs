using System;
using System.Data.SqlTypes;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.Domain.Entities;

namespace ForumAgent.Processors
{
	public class RegisterOrganizationUserProcessor : DefaultCommandProcessor<RegisterOrganizationUser>
	{
		private readonly ISimpleRepository<OrganizationUserEntity> _userRepository;

		public RegisterOrganizationUserProcessor(ISimpleRepository<OrganizationUserEntity> userRepository)
		{
			_userRepository = userRepository;
			AutoMapper.Mapper.CreateMap<RegisterOrganizationUser, OrganizationUserEntity>();
		}

		public override void Process(RegisterOrganizationUser message)
		{
			var domainUser = AutoMapper.Mapper.Map<OrganizationUserEntity>(message);

			// we will generate a password - salt it & hash it & send a notification ot the new user
			domainUser.PasswordHash = "password";
			domainUser.PasswordSalt = "password";

			domainUser.Created = DateTime.Now;
			domainUser.Modified = domainUser.Created;
			domainUser.LastLogin = (DateTime) SqlDateTime.MinValue;

			_userRepository.Save(domainUser);
		}
	}

	public class UpdateOrganizationUserProcessor : DefaultCommandProcessor<UpdateOrganizationUser>
	{
		private readonly ISimpleRepository<OrganizationUserEntity> _userRepository;

		public UpdateOrganizationUserProcessor(ISimpleRepository<OrganizationUserEntity> userRepository)
		{
			_userRepository = userRepository;
			AutoMapper.Mapper.CreateMap<UpdateOrganizationUser, OrganizationUserEntity>()
				.ForMember(
					p => p.Identifier,
					o => o.MapFrom(u => u.UserId))
				.ForMember(p => p.OrganizationEntity, o => o.Ignore());
		}

		public override void Process(UpdateOrganizationUser message)
		{
			var domainUser = _userRepository.FindById(message.UserId);
			domainUser = AutoMapper.Mapper.Map(message, domainUser);

			domainUser.Modified = DateTime.Now;
			_userRepository.Update(domainUser);
		}
	}
}