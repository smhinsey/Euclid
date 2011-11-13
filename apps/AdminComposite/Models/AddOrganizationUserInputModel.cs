using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Models
{
	public class AddOrganizationUserInputModel : DefaultInputModel
	{
		public AddOrganizationUserInputModel()
		{
			AgentSystemName = "NewCo.ForumAgent";
			CommandType = typeof(RegisterOrganizationUser);
		}

		public Guid OrganizationId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Username { get; set; }
	}
}