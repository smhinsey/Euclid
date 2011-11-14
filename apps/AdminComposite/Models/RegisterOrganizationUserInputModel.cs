using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Models
{
	public class RegisterOrganizationUserInputModel : DefaultInputModel
	{
		public RegisterOrganizationUserInputModel()
		{
			AgentSystemName = "NewCo.ForumAgent";
			CommandType = typeof(RegisterOrganizationUser);
		}

		public Guid OrganizationId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public Guid CreatedBy { get; set; }
	}

	public class UpdateOrganizationUserInputModel : DefaultInputModel
	{
		public UpdateOrganizationUserInputModel()
		{
			AgentSystemName = "NewCo.ForumAgent";
			CommandType = typeof(UpdateOrganizationUser);
		}

		public Guid OrganizationId { get; set; }
		public Guid UserId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
	}
}