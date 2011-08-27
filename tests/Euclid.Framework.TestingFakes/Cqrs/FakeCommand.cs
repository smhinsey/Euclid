using System;
using Euclid.Framework.Cqrs;

namespace Euclid.Framework.TestingFakes.Cqrs
{
	public class FakeCommand : ICommand
	{
		public string CommandName { get; set; }

		public DateTime Created { get; set; }

		public Guid CreatedBy { get; set; }

		public Guid Identifier { get; set; }
	}
}