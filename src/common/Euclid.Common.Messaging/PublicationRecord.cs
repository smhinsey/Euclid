using System;

namespace Euclid.Common.Messaging
{
	public class PublicationRecord : IPublicationRecord
	{
		public string CallStack { get; set; }
		public bool Completed { get; set; }
		public DateTime Created { get; set; }
		public Guid CreatedBy { get; set; }
		public bool Dispatched { get; set; }
		public bool Error { get; set; }
		public string ErrorMessage { get; set; }
		public Guid Identifier { get; set; }
		public Uri MessageLocation { get; set; }
		public Type MessageType { get; set; }
	}
}