using System;
using System.Collections.Generic;
using Euclid.Common.Messaging;

namespace Euclid.Common.TestingFakes.Serialization
{
	public class FakeMessage : IMessage
	{
		public DateTime Created { get; set; }
		public Guid CreatedBy { get; set; }
		public IList<string> Field1 { get; set; }
		public int Field2 { get; set; }
		public Guid Identifier { get; set; }
	}
}