using Euclid.Common.Transport;

namespace Euclid.Common.Registry
{
    public interface IRecord<TMessage> : IMessage where TMessage : IMessage
	{
        bool Completed { get; set; }
        bool Error { get; set; }
		string CallStack { get; set; }
		string ErrorMessage { get; set; }
		TMessage Message { get; set; }
	}
}