﻿using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Models
{
	public class ActivateOrganizationUserInputModel : DefaultInputModel
	{
		public ActivateOrganizationUserInputModel()
		{
			CommandType = typeof (ActivateOrganizationUser);
		}

		public Guid UserIdentifier { get; set; }
		public bool Active { get; set; }
	}
}