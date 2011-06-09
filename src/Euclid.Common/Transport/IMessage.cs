using System;

namespace Euclid.Common.Transport
{
	public interface IMessage
	{
		string CallStack { get; set; }
		bool Dispatched { get; set; }
		bool Error { get; set; }
		string ErrorMessage { get; set; }
		Guid Identifier { get; set; }
	}
}