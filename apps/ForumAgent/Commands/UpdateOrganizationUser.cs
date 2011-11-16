using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class UpdateOrganizationUser : DefaultCommand
	{
		public string Email { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public Guid OrganizationId { get; set; }

		public Guid UserId { get; set; }

		public string Username { get; set; }
	}
}