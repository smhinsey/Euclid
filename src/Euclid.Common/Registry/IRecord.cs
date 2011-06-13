using Euclid.Common.Transport;

namespace Euclid.Common.Registry
{
	public interface IRecord<TMessage> : IMessage, IRecord
		where TMessage : IMessage
	{
		TMessage Message { get; set; }
	}

	public interface IRecord
	{
		string CallStack { get; set; }
		bool Completed { get; set; }
		bool Error { get; set; }
		string ErrorMessage { get; set; }
	}
}