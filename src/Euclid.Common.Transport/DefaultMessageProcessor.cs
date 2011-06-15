namespace Euclid.Common.Transport
{
	public abstract class DefaultMessageProcessor<TMessage> : IMessageProcessor<TMessage>
		where TMessage : IMessage
	{
		public abstract void Process(TMessage message);
	}
}