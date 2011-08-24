using System;
using Euclid.Framework.Cqrs;

namespace Euclid.Framework.TestingFakes.Cqrs
{
	public class FakeCommand4 : ICommand
	{
		public DateTime Created { get; set; }

		public Guid CreatedBy { get; set; }

		public Guid Identifier { get; set; }

		public string PasswordHash { get; set; }

		public string PasswordSalt { get; set; }

		public DateTime YourBirthday { get; set; }
	}
}
