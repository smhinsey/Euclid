using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Models
{
	public class RegisterOrganizationUserInputModel : DefaultInputModel
	{
		public RegisterOrganizationUserInputModel()
		{
			CommandType = typeof(RegisterOrganizationUser);
		}

		public Guid CreatedBy { get; set; }

		public string Email { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public Guid OrganizationId { get; set; }

		public string Password { get; set; }

		public string Username { get; set; }
	}
}