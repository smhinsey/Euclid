using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class RegisterForumUser : DefaultCommand
	{
		public string Email { get; set; }

		public string FirstName { get; set; }

		public Guid ForumIdentifier { get; set; }

		public string LastName { get; set; }

		public string PasswordHash { get; set; }

		public string PasswordSalt { get; set; }

		public string Username { get; set; }
	}
}