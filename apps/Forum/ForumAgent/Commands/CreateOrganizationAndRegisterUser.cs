using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class CreateOrganizationAndRegisterUser : DefaultCommand
	{
		public string Address { get; set; }

		public string Address2 { get; set; }

		public string City { get; set; }

		public string Country { get; set; }

		public string Email { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string OrganizationName { get; set; }

		public string OrganizationSlug { get; set; }

		public string OrganizationUrl { get; set; }

		public string PasswordHash { get; set; }

		public string PasswordSalt { get; set; }

		public string PhoneNumber { get; set; }

		public string State { get; set; }

		public string Username { get; set; }

		public string Zip { get; set; }
	}
}