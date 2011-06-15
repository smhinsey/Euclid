namespace Euclid.Common.Transport
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