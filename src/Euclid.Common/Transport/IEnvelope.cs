using System;
using System.IO;

namespace Euclid.Common.Transport
{
	public interface IEnvelope
	{
		string CallStack { get; set; }
		bool Dispatched { get; set; }
		bool Error { get; set; }
		string ErrorMessage { get; set; }
		Guid Identifier { get; set; }
		Stream Message { get; set; }
		Type MessageType { get; set; }
	}
}