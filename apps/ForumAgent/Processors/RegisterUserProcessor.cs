using System;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
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
					Username = message.Username,
					Created = DateTime.Now,
					Modified = DateTime.Now,
					ForumIdentifier = message.ForumIdentifier
				};

			_repository.Save(newUser);
		}
	}
}