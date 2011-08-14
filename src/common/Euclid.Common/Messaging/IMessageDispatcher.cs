namespace Euclid.Common.Messaging
{
	public interface IMessageDispatcher
	{
		IMessageDispatcherSettings CurrentSettings { get; }
		MessageDispatcherState State { get; }
		void Configure(IMessageDispatcherSettings settings);
		void Disable();
		void Enable();
	}
}