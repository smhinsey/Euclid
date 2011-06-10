using System;
using System.Collections.Generic;
using Euclid.Common.Transport;

namespace Euclid.Common.TestingFakes.Serialization
{
	public class FakeMessage : IMessage
	{
		public IList<string> Field1 { get; set; }
		public int Field2 { get; set; }
		public Guid Identifier { get; set; }
	    public Guid CreatedBy { get; set; }
	    public DateTime Created { get; set; }
	}
}