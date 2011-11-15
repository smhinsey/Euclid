using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Models
{
	public class UpdateOrganizationInputModel : DefaultInputModel
	{
		public UpdateOrganizationInputModel()
		{
			AgentSystemName = "NewCo.ForumAgent";
			CommandType = typeof(UpdateOrganization);
		}

		public Guid OrganizationIdentifier { get; set; }
		public string OrganizationName { get; set; }
		public string OrganizationSlug { get; set; }
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