namespace Euclid.Common.Messaging
{
	/// <summary>
	/// 	Implementations of IMessageProcessor can be installed into a message dispatcher
	/// 	where they will be wired up to receive messages from a specified IMessageChannel.
	/// </summary>
	/// <typeparam name = "TMessage"></typeparam>
	public interface IMessageProcessor<in TMessage> : IMessageProcessor
		where TMessage : IMessage
	{
		void Process(TMessage message);
	}

	public interface IMessageProcessor
	{
		
	}
}