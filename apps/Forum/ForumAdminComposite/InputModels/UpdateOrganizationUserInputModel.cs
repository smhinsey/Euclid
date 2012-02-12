using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.InputModels
{
	public class UpdateOrganizationUserInputModel : DefaultInputModel
	{
		public UpdateOrganizationUserInputModel()
		{
			CommandType = typeof(UpdateOrganizationUser);
		}

		public string Email { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public Guid OrganizationId { get; set; }

		public string Password { get; set; }

		public Guid UserId { get; set; }

		public string Username { get; set; }
	}
}