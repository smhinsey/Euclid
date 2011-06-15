namespace Euclid.Common.Transport
{
	/// <summary>
	/// 	Implementations of IMessageProcessor can be installed into a message dispatcher
	/// 	where they will be wired up to receive messages from a specified IMessageTransport.
	/// </summary>
	/// <typeparam name = "TMessage"></typeparam>
	public interface IMessageProcessor<in TMessage>
		where TMessage : IMessage
	{
		void Process(TMessage message);
	}
}