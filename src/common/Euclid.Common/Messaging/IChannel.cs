using System;
using System.Collections.Generic;

namespace Euclid.Common.Messaging
{
	/// <summary>
	/// 	Represents a message channel over which messages can be sent and received. Typically implements integration with
	/// 	a third party service such as MSMQ, Azure Queues, etc.
	/// </summary>
	/// <typeparam name = "TMessage">The type of message which travels over this channel. This could be an interface such as IMessage, for 
	/// 	channels which are intended for all message types, or a user-provided MyMessage type.</typeparam>
	public interface IChannel<TMessage>
		where TMessage : IMessage
	{
		/// <summary>
		/// 	Gets or sets a friendly name for the channel. Depending on the implementation this may include information about
		/// 	the channel's connection string or other similar connection specification mechanism.
		/// </summary>
		string ChannelName { get; set; }

		/// <summary>
		/// 	Gets the channel's current state.
		/// </summary>
		ChannelState State { get; }

		/// <summary>
		/// 	Removes all messages from the channel, regardless of their content.
		/// </summary>
		void Clear();

		/// <summary>
		/// 	Closes the current channel. A closed channel cannot be used for Send or Receive operations.
		/// </summary>
		/// <returns>The updated channel state.</returns>
		ChannelState Close();

		/// <summary>
		/// 	Opens a channel so that messages can be sent and received.
		/// </summary>
		/// <returns>The updated channel state.</returns>
		ChannelState Open();

		/// <summary>
		/// 	Receives one or more messages during the specified timespan.
		/// </summary>
		/// <param name = "howMany">The maximum number of messages to receive.</param>
		/// <param name = "timeSpan">Assuming the maximum number of messages have not been received, how long to wait for more messages.</param>
		/// <returns>The received messages, up to howMany.</returns>
		IEnumerable<TMessage> ReceiveMany(int howMany, TimeSpan timeSpan);

		/// <summary>
		/// 	Receives a single message and returns it.
		/// </summary>
		/// <param name = "timeSpan">How long to wait for a message if none is available immediately.</param>
		/// <returns>The received message.</returns>
		TMessage ReceiveSingle(TimeSpan timeSpan);

		/// <summary>
		/// 	Sends a message over the channel.
		/// </summary>
		/// <param name = "message">The message to send.</param>
		void Send(TMessage message);
	}
}