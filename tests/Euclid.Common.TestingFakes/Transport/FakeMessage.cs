using System;
using System.IO;
using Euclid.Common.Transport;

namespace Euclid.Common.TestingFakes.Transport
{
	public class FakeMessage : IEnvelope
	{
		public FakeMessage()
		{
			Identifier = Guid.NewGuid();
		}

		public string CallStack { get; set; }

		public bool Dispatched { get; set; }
		public bool Error { get; set; }
		public string ErrorMessage { get; set; }
		public Guid Identifier { get; set; }
		public Stream Message { get; set; }
		public Type MessageType { get; set; }
	}
}