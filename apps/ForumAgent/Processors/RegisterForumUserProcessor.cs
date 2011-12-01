using System;
using System.Data.SqlTypes;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class RegisterForumUserProcessor : DefaultCommandProcessor<RegisterForumUser>
	{
		private readonly ISimpleRepository<ForumUser> _repository;

		public RegisterForumUserProcessor(ISimpleRepository<ForumUser> repository)
		{
			_repository = repository;
		}

		public override void Process(RegisterForumUser message)
		{
			var newUser = new ForumUser
				{
					FirstName = message.FirstName,
					LastName = message.LastName,
					Email = message.Email,
					PasswordHash = message.PasswordHash,
					PasswordSalt = message.PasswordSalt,
					Username = message.Username,
					Created = DateTime.Now,
					Modified = DateTime.Now,
					ForumIdentifier = message.ForumIdentifier,
					LastLogin = (DateTime)SqlDateTime.MinValue,
					Active = true
				};

			_repository.Save(newUser);
		}
	}
}