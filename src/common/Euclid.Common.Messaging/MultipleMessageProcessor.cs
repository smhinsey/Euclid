namespace Euclid.Common.Messaging
{
	public abstract class MultipleMessageProcessor : IMessageProcessor
	{
		public bool CanProcessMessage(IMessage message)
		{
			// check for any method with a single argument of the same type as message

			return true;
		}
	}
}