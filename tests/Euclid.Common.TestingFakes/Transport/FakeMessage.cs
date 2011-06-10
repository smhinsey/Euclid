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

		public int Field1 { get; set; }
		public IList<string> Field2 { get; set; }
		public Guid Identifier { get; set; }
	    public Guid CreatedBy { get; set; }
	    public DateTime Created { get; set; }
	}
}