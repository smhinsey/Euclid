namespace Euclid.Common.Transport
{
	public interface IMessageDispatcher
	{
		void Configure(IMessageDispatcherSettings settings);
		void Enable();
		void Disable();
		MessageDispatcherState State { get; }
		IMessageDispatcherSettings CurrentSettings { get; }
	}
}