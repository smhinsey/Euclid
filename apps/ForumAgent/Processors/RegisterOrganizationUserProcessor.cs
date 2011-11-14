using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.WriteModels;

namespace ForumAgent.Processors
{
	public class RegisterOrganizationUserProcessor : DefaultCommandProcessor<RegisterOrganizationUser>
	{
		private readonly ISimpleRepository<DomainOrganizationUser> _userRepository;

		public RegisterOrganizationUserProcessor(ISimpleRepository<DomainOrganizationUser> userRepository)
		{
			_userRepository = userRepository;
			AutoMapper.Mapper.CreateMap<RegisterOrganizationUser, DomainOrganizationUser>();
		}

		public override void Process(RegisterOrganizationUser message)
		{
			var domainUser = AutoMapper.Mapper.Map<DomainOrganizationUser>(message);

			// we will generate a password - salt it & hash it & send a notification ot the new user
			domainUser.PasswordHash = "password";
			domainUser.PasswordSalt = "password";

			_userRepository.Save(domainUser);
		}
	}
}