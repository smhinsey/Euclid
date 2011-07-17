using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ProfileAgent.Commands;
using ProfileAgent.ReadModel;

namespace ProfileAgent.Processors
{
	// SELF this pattern is obviously repetitious, we need to figure out a more streamlined approach for basic CRUD commands
	public class RegisterUserProcessor : DefaultCommandProcessor<RegisterUser>
	{
		private readonly ISimpleRepository<User> _repository;

		public RegisterUserProcessor(ISimpleRepository<User> repository)
		{
			_repository = repository;
		}

		public override void Process(RegisterUser message)
		{
			var newUser = new User
			              	{
			              		PasswordHash = message.PasswordHash,
			              		PasswordSalt = message.PasswordSalt,
			              		Username = message.Username
			              	};

			_repository.Save(newUser);
		}
	}
}