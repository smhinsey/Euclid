using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class CreateOrganization : DefaultCommand
	{
		public string OrganizationName { get; set; }
		public string OrganizationUrl { get; set; }
		public string PhoneNumber { get; set; }
		public string Address { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }
		public string Country { get; set; }
	}
}