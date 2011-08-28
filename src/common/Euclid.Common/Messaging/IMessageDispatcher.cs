namespace Euclid.Common.Messaging
{
	/// <summary>
	/// 	Dispatches messages to relevant configured processors, which are supplied via IMessageDispatcherSettings.
	/// </summary>
	public interface IMessageDispatcher
	{
		/// <summary>
		/// 	Gets the dispatcher's currently active settings.
		/// </summary>
		IMessageDispatcherSettings CurrentSettings { get; }

		/// <summary>
		/// 	Gets the dispatcher's current state.
		/// </summary>
		MessageDispatcherState State { get; }

		/// <summary>
		/// 	Configures a dispatcher with an input channel, processors, and other relevant settings.
		/// </summary>
		/// <param name = "settings">The settings to configure.</param>
		void Configure(IMessageDispatcherSettings settings);

		/// <summary>
		/// 	Disables the dispatcher, meaning it will no longer dispatch messages from its input channel.
		/// </summary>
		void Disable();

		/// <summary>
		/// 	Enables the dispatcher, causing it to begin processing messages on its input channel.
		/// </summary>
		void Enable();
	}
}