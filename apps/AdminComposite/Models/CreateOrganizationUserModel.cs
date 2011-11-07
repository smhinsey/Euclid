using System;

namespace AdminComposite.Models
{
	public class CreateOrganizationUserModel
	{
		public bool DisplayTitle { get; set; }
		public bool ContactInfoRequired { get; set; }
		public Guid OrganizationIdentifier { get; set; }
	}
}