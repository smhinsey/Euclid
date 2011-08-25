using System;
using Euclid.Common.Messaging;

namespace Euclid.Common.TestingFakes.Registry
{
	public class FakeMessage : IMessage
	{
		public DateTime Created { get; set; }

		public Guid CreatedBy { get; set; }

		public Guid Identifier { get; set; }
	}
}
