namespace Euclid.Common.Messaging
{
	public abstract class MessageProcessorBase<TMessage> : IMessageProcessor<TMessage>
		where TMessage : IMessage
	{
		public abstract void Process(TMessage message);

		public bool CanProcessMessage(IMessage message)
		{
			return (message.GetType() == typeof (TMessage));
		}
	}
}