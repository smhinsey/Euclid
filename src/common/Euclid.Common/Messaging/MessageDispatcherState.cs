namespace Euclid.Common.Messaging
{
	/// <summary>
	/// 	Indicates the runtime state of a message dispatcher.
	/// </summary>
	public enum MessageDispatcherState
	{
		/// <summary>
		/// 	The dispatcher is enabled and processing messages.
		/// </summary>
		Enabled,

		/// <summary>
		/// 	The dispatcher is disabled and not currently processing messages.
		/// </summary>
		Disabled
	}
}