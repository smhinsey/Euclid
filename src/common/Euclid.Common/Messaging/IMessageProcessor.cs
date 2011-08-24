namespace Euclid.Common.Messaging
{
	/// <summary>
	/// 	Implementations of IMessageProcessor can be installed into a message dispatcher
	/// 	where they will be wired up to receive messages from a specified IMessageChannel.
	/// </summary>
	/// <typeparam name = "TMessage">Message to be processed</typeparam>
	public interface IMessageProcessor<in TMessage> : IMessageProcessor
		where TMessage : IMessage
	{
		/// <summary>
		/// 	Processes the individual message.
		/// </summary>
		/// <param name = "message">The message to process.</param>
		void Process(TMessage message);
	}

	/// <summary>
	/// 	Base interface for message processors. This allows for the potential to implement processors
	/// 	in different styles.
	/// </summary>
	public interface IMessageProcessor
	{
		/// <summary>
		/// 	Indicates whether the processor is capable of processing the message in question.
		/// </summary>
		/// <param name = "message">The message in question.</param>
		/// <returns>Whether or not the processor instance can process the message.</returns>
		bool CanProcessMessage(IMessage message);
	}
}
