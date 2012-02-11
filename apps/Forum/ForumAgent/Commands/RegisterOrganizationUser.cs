using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class RegisterOrganizationUser : DefaultCommand
	{
		public string Email { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public Guid OrganizationId { get; set; }

		public string PasswordHash { get; set; }

		public string PasswordSalt { get; set; }

		public string Username { get; set; }
	}
}