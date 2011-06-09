using System;
using System.Collections.Generic;
using Euclid.Common.Transport;

namespace Euclid.Common.TestingFakes.Transport
{
	public class FakeMessage : IMessage
	{
		public FakeMessage()
		{
			Identifier = Guid.NewGuid();
		}

		public string CallStack { get; set; }

		public bool Dispatched { get; set; }
		public bool Error { get; set; }
		public string ErrorMessage { get; set; }

		public int Field1 { get; set; }
		public IList<string> Field2 { get; set; }
		public Guid Identifier { get; set; }
	}
}