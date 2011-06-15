namespace Euclid.Common.Transport
{
	public interface IMessageDispatcher
	{
		void Configure(IMessageTransport inputTransport);
		void Enable();
		void Disable();
		MessageDispatcherState State { get; }
	}
}