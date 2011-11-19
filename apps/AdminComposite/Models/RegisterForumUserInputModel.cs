﻿using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Models
{
	public class RegisterForumUserInputModel : DefaultInputModel
	{
		public RegisterForumUserInputModel()
		{
			AgentSystemName = "NewCo.ForumAgent";
			CommandType = typeof(RegisterForumUser);
		}

		public string ConfirmationPassword { get; set; }

		public string Email { get; set; }

		public string FirstName { get; set; }

		public Guid ForumIdentifier { get; set; }

		public string LastName { get; set; }

		public string Password { get; set; }

		public string Username { get; set; }

		public Guid CreatedBy { get; set; }
	}
}