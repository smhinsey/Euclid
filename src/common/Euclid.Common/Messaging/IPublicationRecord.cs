using System;

namespace Euclid.Common.Messaging
{
	public interface IPublicationRecord : IMessage
	{
		string CallStack { get; set; }
		bool Completed { get; set; }
		bool Dispatched { get; set; }
		bool Error { get; set; }
		string ErrorMessage { get; set; }
		Uri MessageLocation { get; set; }
		Type MessageType { get; set; }
	}
}