namespace Euclid.Common.Messaging
{
	/// <summary>
	/// 	Indicates the runtime state of a channel.
	/// </summary>
	public enum ChannelState
	{
		/// <summary>
		/// 	The channel is not configured.
		/// </summary>
		NotConfigured, 

		/// <summary>
		/// 	The channel is open.
		/// </summary>
		Open, 

		/// <summary>
		/// 	The channel is closed.
		/// </summary>
		Closed, 
	}
}
