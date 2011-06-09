using System;
using System.Collections.Generic;
using Euclid.Common.Transport;

namespace Euclid.Common.TestingFakes.Serialization
{
	public class FakeMessage : IMessage
	{
		public string CallStack { get; set; }
		public bool Dispatched { get; set; }
		public bool Error { get; set; }
		public string ErrorMessage { get; set; }

		public IList<string> Field1 { get; set; }
		public int Field2 { get; set; }
		public Guid Identifier { get; set; }
	}
}