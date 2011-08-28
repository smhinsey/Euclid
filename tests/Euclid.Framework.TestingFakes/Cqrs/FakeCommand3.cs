using System;
using Euclid.Framework.Cqrs;

namespace Euclid.Framework.TestingFakes.Cqrs
{
	public class FakeCommand3 : ICommand
	{
		public DateTime Created { get; set; }

		public Guid CreatedBy { get; set; }

		public Guid Identifier { get; set; }
	}
}