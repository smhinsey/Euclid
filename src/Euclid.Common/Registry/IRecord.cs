using System;
using Euclid.Common.Transport;

namespace Euclid.Common.Registry
{
	public interface IRecord : IMessage
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