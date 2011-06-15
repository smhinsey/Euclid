using System;
using Euclid.Common.Registry;

namespace Euclid.Common.TestingFakes.Registry
{
	public class FakeRecord : IRecord
	{
		public string CallStack { get; set; }
		public bool Completed { get; set; }
		public DateTime Created { get; set; }
		public Guid CreatedBy { get; set; }
		public bool Error { get; set; }
		public string ErrorMessage { get; set; }
	    public Uri MessageLocation { get; set; }
	    public Type MessageType { get; set; }
	    public Guid Identifier { get; set; }
	}
}