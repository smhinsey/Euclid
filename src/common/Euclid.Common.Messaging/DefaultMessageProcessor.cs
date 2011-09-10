namespace Euclid.Common.Messaging
{
	public abstract class DefaultMessageProcessor<TMessage> : IMessageProcessor<TMessage>
		where TMessage : IMessage
	{
		public bool CanProcessMessage(IMessage message)
		{
			return message.GetType() == typeof(TMessage);
		}

		public abstract void Process(TMessage message);
	}
}