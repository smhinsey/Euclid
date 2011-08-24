﻿using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class RegisterUser : DefaultCommand
	{
		public string PasswordHash { get; set; }

		public string PasswordSalt { get; set; }

		public string Username { get; set; }
	}
}
