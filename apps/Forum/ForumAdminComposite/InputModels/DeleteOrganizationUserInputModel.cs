using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.InputModels
{
	public class DeleteOrganizationUserInputModel : DefaultInputModel
	{
		public DeleteOrganizationUserInputModel()
		{
			CommandType = typeof (DeleteOrganizationUser);
		}

		public Guid UserIdentifier { get; set; }
	}
}